namespace Shipwreck.ViewModelUtils
{
    internal static partial class StringHelper
    {
        public static string EmptyToNull(this string s)
            => string.IsNullOrEmpty(s) ? null : s;

        public static string GetTitle(string title, string mnemonic)
        {
            var t = title;
            GetTitle(ref t, mnemonic);
            return t;
        }

        static partial void GetTitle(ref string title, string mnemonic);
    }
}
