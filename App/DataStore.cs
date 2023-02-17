namespace IdCardReaderThales
{
    public static class DataStore
    {
        public static PassportScanner ps;

        static DataStore()
        {
            ps = new PassportScanner();
        }
    }
}
