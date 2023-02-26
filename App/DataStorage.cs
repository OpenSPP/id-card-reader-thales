namespace IdCardReaderThales
{
    public static class DataStorage
    {
        public static PassportScanner ps;

        static DataStorage()
        {
            ps = new PassportScanner();
        }
    }
}
