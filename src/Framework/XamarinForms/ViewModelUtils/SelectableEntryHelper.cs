namespace Shipwreck.ViewModelUtils;

public static class SelectableEntryHelper
{
    public static bool HandleKeyDown(this SelectableEntry entry, string keys, bool replaceText)
    {
        if (entry.IsVisible)
        {
            var lines = keys.Split('\r', '\n');
            var fl = lines.FirstOrDefault();

            if (!string.IsNullOrEmpty(fl))
            {
                var func = entry.Keyboard != Keyboard.Numeric
                            && entry.Keyboard != Keyboard.Telephone
                            ? c => '0' <= c && c <= '9' || 'a' <= c && c <= 'z'
                            : (Func<char, bool>)(c => '0' <= c && c <= '9');

                var nt = new string(fl.Where(func).ToArray());

                string text;
                int cp, sl;
                if (replaceText)
                {
                    text = nt;
                    cp = 0;
                    sl = text.Length;
                }
                else if (entry.IsFocused)
                {
                    text = entry.Text;
                    cp = entry.CursorPosition;
                    sl = entry.SelectionLength;
                    text = text.Substring(0, cp) + nt + text.Substring(cp + sl);
                    cp += nt.Length;
                    sl = 0;
                }
                else
                {
                    text = entry.Text + nt;
                    cp = text.Length;
                    sl = 0;
                }

                entry.Text = text;
                entry.CursorPosition = cp;
                entry.SelectionLength = sl;
            }

            if (lines.Length > 1)
            {
                entry.SendCompleted();
            }

            return true;
        }

        return false;
    }
}
