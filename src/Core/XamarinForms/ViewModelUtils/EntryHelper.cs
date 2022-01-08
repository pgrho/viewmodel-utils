namespace Shipwreck.ViewModelUtils;

public static class EntryHelper
{
    public static bool HandleKeyDown(this Entry entry, string keys)
    {
        if (entry.IsVisible)
        {
            entry.Focus();

            Func<char, bool> appendPredicate;

            if (entry.Keyboard == Keyboard.Numeric || entry.Keyboard == Keyboard.Telephone)
            {
                appendPredicate = c => '0' <= c && c <= '9';
            }
            else
            {
                appendPredicate = c => ('0' <= c && c <= '9') || ('a' <= c && c <= 'z');
            }

            foreach (var c in keys)
            {
                if (c == '\r' || c == '\n')
                {
                    entry.ReturnCommand?.Execute(entry.ReturnCommandParameter);
                    break;
                }
                else if (appendPredicate(c))
                {
                    entry.Text += c;
                }
            }
            return true;
        }
        return false;
    }
}
