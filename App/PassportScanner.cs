using System.Collections;
using System.Drawing;
using System.Globalization;
using BerTlv;

namespace IdCardReaderThales
{

    public class PassportScanner
    {
        public PassportScanner()
        {
        }

        public void initialise()
        {
            if (MMM.Readers.FullPage.Reader.GetState() == MMM.Readers.FullPage.ReaderState.READER_NOT_INITIALISED)
            {
            MMM.Readers.FullPage.Reader.EnableLogging(
            true,
            1,
            -1,
            "IdCardReaderThales"
            );

            MMM.Readers.ErrorCode result = MMM.Readers.FullPage.Reader.Initialise(
                null,
                null,
                null,
                null,
                true,
                false
            );

            if (result != MMM.Readers.ErrorCode.NO_ERROR_OCCURRED)
            {
                throw new Exception(string.Format("An error occurred during initialisation - {0}", result.ToString()));
            }
            };

        }

        public Hashtable readDocument()
        {
            lock(this)
            { 
                Hashtable info = new Hashtable();
                MMM.Readers.FullPage.ReaderSettings lSettings;
                MMM.Readers.ErrorCode lErrorCode =
                    MMM.Readers.FullPage.Reader.GetSettings(out lSettings);
                lSettings.puDataToSend.send |= MMM.Readers.FullPage.DataSendSet.Flags.SMARTCARD;
                lSettings.puDataToSend.send |= MMM.Readers.FullPage.DataSendSet.Flags.VISIBLEIMAGE;
                lSettings.puRFIDSettings.puRFProcessSettings.puRFApplicationMode = 1;
                lSettings.puDataToSend.rfid.puDGFile[11] = 1;
                lSettings.puDataToSend.rfid.puDG2FaceJPEG = 1;
                MMM.Readers.FullPage.Reader.UpdateSettings(lSettings);

                MMM.Readers.ErrorCode result = MMM.Readers.FullPage.Reader.ReadDocument();
                if (result != MMM.Readers.ErrorCode.NO_ERROR_OCCURRED)
                {
                    throw new Exception(string.Format("An error occurred reading the document - {0}", result.ToString()));
                }
                else
                {
                    // All data is passed back as an object, which should be cast to the
                    // correct type to be used
                    object? data = null;

                    result = MMM.Readers.FullPage.Reader.GetData(
                        MMM.Readers.FullPage.DataType.CD_CODELINE_DATA,
                        ref data
                    );

                    if (result == MMM.Readers.ErrorCode.NO_ERROR_OCCURRED)
                    {
                        MMM.Readers.CodelineData codeline = (MMM.Readers.CodelineData)data;
                    //    info["given_name"] = codeline.Forenames;
                    //    info["family_name"] = codeline.Surname;
                        info["gender"] = codeline.Sex;
                        info["document_type"] = codeline.DocType;
                        info["document_number"] = codeline.DocNumber;
                        info["birth_date"] = string.Format(
                                        "{0:0000}-{1:00}-{2:00}",
                                        CultureInfo.CurrentCulture.Calendar.ToFourDigitYear(codeline.DateOfBirth.Year),
                                        codeline.DateOfBirth.Month,
                                        codeline.DateOfBirth.Day
                                    );
                        info["expiry_date"] = string.Format(
                                        "{0:0000}-{1:00}-{2:00}",
                                        CultureInfo.CurrentCulture.Calendar.ToFourDigitYear(codeline.ExpiryDate.Year),
                                        codeline.ExpiryDate.Month,
                                        codeline.ExpiryDate.Day
                                    );
                    //    info["nationality"] = codeline.Nationality;
                    //    //info["address"] =
                    //    //info["birth_place"] =
                    //    //info["issue_date"] =
                    }
                    result = MMM.Readers.FullPage.Reader.GetData(
                        MMM.Readers.FullPage.DataType.CD_SCDG11_FILE,
                        ref data
                    );
                    if (result == MMM.Readers.ErrorCode.NO_ERROR_OCCURRED)
                    {
                        byte[]? lFileData = data as byte[];

                        //parse the data 

                        ICollection<Tlv> tlvs = Tlv.ParseTlv(lFileData);

                        Console.WriteLine("TLVS");
                        Console.WriteLine(tlvs.Count);
                        foreach (Tlv tlv in tlvs)
                        {
                            Console.WriteLine(tlv.Tag);
                            foreach (Tlv tlv2 in tlv.Children)
                            {
                                Console.WriteLine(tlv2.HexTag);
                                Console.WriteLine(tlv2.Children.Count);

                                String value = System.Text.Encoding.UTF8.GetString(tlv2.Value);
                                Console.WriteLine(value);
                                Console.WriteLine(tlv2.HexValue);

                                if (tlv2.HexTag == "5F0E")
                                {
                                    info["name"] = value.Replace("<"," ");
                                    String[] separator = { "<<" };
                                    String[] strlist = value.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                                    
                                    if (strlist[0].Contains("<"))
                                    {
                                        String[] sep2 = { "<" };
                                        String[] str2list = strlist[0].Split(sep2, StringSplitOptions.RemoveEmptyEntries);
                                        info["grand_father_name"] = str2list[2].Replace("<", " "); ;
                                        info["father_name"] = str2list[1].Replace("<", " "); ;
                                        info["family_name"] = strlist[1].Replace("<", " "); ;
                                        info["given_name"] = str2list[0].Replace("<", " "); ;
                                    } else
                                    {
                                        info["given_name"] = strlist[1].Replace("<", " "); ;
                                        info["family_name"] = strlist[0].Replace("<", " "); ;
                                    }
                                    

                                } else if (tlv2.HexTag == "5F10")   
                                {
                                    info["document_number"] = value;
                                }
                                else if (tlv2.HexTag == "5F11")
                                {
                                    if (value.Contains("<"))
                                    {
                                        String[] spearator = { "<" };
                                        String[] strlist = value.Split(spearator, StringSplitOptions.RemoveEmptyEntries);
                                        info["birth_place_city"] = strlist[0];
                                        info["birth_place_country"] = strlist[1];
                                    } else
                                    {
                                        info["birth_place_city"] = value;
                                    }
                                }
                                else if (tlv2.HexTag == "5F42")
                                {
                                    info["address"] = value;
                                }

                            }
                        }

                        Console.WriteLine(tlvs);


                    }

                    result = MMM.Readers.FullPage.Reader.GetData(
                        MMM.Readers.FullPage.DataType.CD_IMAGEVIS,
                        ref data
                    );


                    if (result == MMM.Readers.ErrorCode.NO_ERROR_OCCURRED)
                    {
                        Bitmap? bImage = data as Bitmap;
                        if ((bImage != null) && (OperatingSystem.IsWindows()))
                        {
                            System.IO.MemoryStream ms = new MemoryStream();
                            bImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                            byte[] byteImage = ms.ToArray();
                            string base64String = Convert.ToBase64String(byteImage);
                            info["image"] = base64String;
                        }
                    }

                    //result = MMM.Readers.FullPage.Reader.GetData(
                    //    MMM.Readers.FullPage.DataType.CD_IMAGEPHOTO,
                    //    ref data
                    //);

                    //if (result == MMM.Readers.ErrorCode.NO_ERROR_OCCURRED)
                    //{
                    //    Bitmap bImage = data as Bitmap;
                    //    System.IO.MemoryStream ms = new MemoryStream();
                    //    bImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    //    byte[] byteImage = ms.ToArray();
                    //    string base64String = Convert.ToBase64String(byteImage);
                    //    //info["photo"] = base64String;
                    //}

                    result = MMM.Readers.FullPage.Reader.GetData(
                        MMM.Readers.FullPage.DataType.CD_SCDG2_PHOTO,
                        ref data
                    );

                    if (result == MMM.Readers.ErrorCode.NO_ERROR_OCCURRED)
                    {
                        byte[]? byteImage = data as byte[];
                        byte[]? OutputImage = null;
                        if (byteImage != null)
                        {
                            MMM.Readers.Modules.Imaging.ConvertFormat
                                                        (MMM.Readers.FullPage.ImageFormats.RTE_JPEG,
                                                        byteImage, out OutputImage);
                            string base64String = Convert.ToBase64String(OutputImage);
                            info["photo"] = base64String;
                        }
                    }
                }
                return info;
            }
        }

        public Hashtable qrcode()
        {
            Hashtable info = new Hashtable();
            object? data = null;
            MMM.Readers.FullPage.PluginData pluginData;

            MMM.Readers.FullPage.Reader.EnablePlugin("QRCode", true);
            MMM.Readers.FullPage.ReaderSettings lSettings;
            MMM.Readers.ErrorCode lErrorCode =
                MMM.Readers.FullPage.Reader.GetSettings(out lSettings);
            lSettings.puDataToSend.send |= MMM.Readers.FullPage.DataSendSet.Flags.SMARTCARD;
            lSettings.puDataToSend.send |= MMM.Readers.FullPage.DataSendSet.Flags.BARCODEIMAGE;
            lSettings.puImageSettings.useVisibleForBarcode = 1;
            MMM.Readers.FullPage.Reader.UpdateSettings(lSettings);


            MMM.Readers.ErrorCode result = MMM.Readers.FullPage.Reader.ReadDocument();

            result = MMM.Readers.FullPage.Reader.GetData(
                MMM.Readers.FullPage.DataType.CD_IMAGEVIS,
                ref data
            );

            if (result == MMM.Readers.ErrorCode.NO_ERROR_OCCURRED)
            {
                Bitmap? bImage = data as Bitmap;
                if ((bImage != null) && (OperatingSystem.IsWindows()))
                {
                    System.IO.MemoryStream ms = new MemoryStream();
                    bImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] byteImage = ms.ToArray();
                    string base64String = Convert.ToBase64String(byteImage);
                    //info["image"] = base64String;
                }
            }

            result = MMM.Readers.FullPage.Reader.GetPluginData(
                "QRCode",
                0,
                0,
                out pluginData
            );

            if (result == MMM.Readers.ErrorCode.NO_ERROR_OCCURRED)
            {
                if (pluginData.puDataFormat == MMM.Readers.FullPage.DataFormat.DF_STRING_ASCII)
                {
                    string? qrcodedata = pluginData.puData.ToString();
                    info["qrcode"] = qrcodedata;
                }
                else if (pluginData.puDataFormat == MMM.Readers.FullPage.DataFormat.DF_BINARYDATA)
                {
                    byte[] qrcodedata = (byte[])pluginData.puData;
                    info["qrcode"] = qrcodedata;
                } else
                {
                    info["error"] = "No QR code detected";
                }
                
            }

            return info;
        }

        public void shutDown()
        {
            MMM.Readers.FullPage.Reader.Reset();
        }
    }
}
