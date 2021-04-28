namespace Shipwreck
{
    internal static class SH
    {
        public static string TrimOrEmpty(this string s)
            => s?.TrimEnd();
    }
}
