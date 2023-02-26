using System.Collections;

namespace IdCardReaderThales
{
    public static class API
    {
        public static IResult Initialise()
        {
            try
            {
                Hashtable response = new Hashtable();
                response["initialise"] = "true";
                DataStorage.ps.initialise();
                return Results.Json(response);
            }
            catch (Exception e)
            {
                Hashtable response = new Hashtable();

                response["error"] = e.Message;
                return Results.Json(response);
            }
        }

        public static IResult Shutdown()
        {
            try
            {
                Hashtable response = new Hashtable();
                response["shutdown"] = "true";
                DataStorage.ps.shutDown();
                return Results.Json(response);
            }
            catch (Exception e)
            {
                Hashtable response = new Hashtable();

                response["error"] = e.Message;
                return Results.Json(response);
            }
        }

        public static IResult ReadDocument()
        {
            try
            {
                return Results.Json(DataStorage.ps.readDocument());
            }
            catch (Exception e)
            {
                Hashtable response = new Hashtable();

                response["error"] = e.Message;
                return Results.Json(response);
            }
        }

        public static IResult QrCode()
        {
            try
            {
                return Results.Json(DataStorage.ps.qrcode());
            }
            catch (Exception e)
            {
                Hashtable response = new Hashtable();

                response["error"] = e.Message;
                return Results.Json(response);
            }
        }
    }
}
