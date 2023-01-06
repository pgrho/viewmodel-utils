namespace Shipwreck.ViewModelUtils;

public static class SelectableEntryHelper
{
    public static bool FocusOrSelectAll(this SelectableEntry entry)
    {
        if (entry.IsFocused || entry.Focus())
        {
            entry.CursorPosition = 0;
            entry.SelectionLength = entry.Text?.Length ?? 0;

            return true;
        }
        return false;
    }

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
                int cursorPosition, selectionLength;
                if (replaceText)
                {
                    text = nt;
                    cursorPosition = 0;
                    selectionLength = text.Length;
                }
                else if (entry.IsFocused)
                {
                    text = entry.Text;
                    cursorPosition = entry.CursorPosition;
                    selectionLength = entry.SelectionLength;
                    text = text.Substring(0, cursorPosition) + nt + text.Substring(cursorPosition + selectionLength);
                    cursorPosition += nt.Length;
                    selectionLength = 0;
                }
                else
                {
                    text = entry.Text + nt;
                    cursorPosition = text.Length;
                    selectionLength = 0;
                }

                entry.Text = text;
                if (lines.Length > 1)
                {
                    entry.SendCompleted();
                    return true;
                }

                if (entry.IsFocused)
                {
                    entry.CursorPosition = cursorPosition;
                    entry.SelectionLength = selectionLength;
                }
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
