namespace Shipwreck.ViewModelUtils
{
    internal static partial class StringHelper
    {
        static partial void GetTitle(ref string title, string mnemonic)
            => title = string.IsNullOrEmpty(mnemonic) ? title : $"{title} (_{mnemonic})";
    }
}
