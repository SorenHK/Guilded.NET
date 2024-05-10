namespace System
{
#if NETSTANDARD2_0

    public static class StringUtils
    {
        public static bool StartsWith(this string str, char c)
        {
            return str.Length != 0 && str[0] == c;
        }
    }

#endif
}