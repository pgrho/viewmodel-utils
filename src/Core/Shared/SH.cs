namespace Shipwreck;

    internal static class SH
    {
        public static string TrimOrNull(this string s)
            => s != null ? s.Trim() : null;
}
