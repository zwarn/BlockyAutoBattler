namespace util
{
    public static class IntegerExtension
    {
        // Positive Modulo
        public static int Mod(this int value, int mod)
        {
            int r = value % mod;
            return r < 0 ? r + mod : r;
        }
    }
}