namespace Shipwreck.ViewModelUtils.Controls;

public static class TextBoxBehavior
{
    public static bool Focus(this TextBox tb, bool shouldSelect = false)
    {
        if (tb.Focus())
        {
            if (shouldSelect)
            {
                tb.SelectionStart = 0;
                tb.SelectionLength = tb.Text.Length;
            }
            return true;
        }
        return false;
    }
}
