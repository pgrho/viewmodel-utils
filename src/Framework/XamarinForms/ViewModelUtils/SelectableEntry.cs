using Shipwreck.XamarinFormsRenderers;

namespace Shipwreck.ViewModelUtils;

public class SelectableEntry : ExtendedEntry, IKeyDownHandler
{
    public SelectableEntry()
    {
        SelectAllOnFocus = true;
        ReturnType = ReturnType.Done;
    }

    bool IKeyDownHandler.GetIsFocused() => IsFocused;

    public bool HandleKeyDown(string keys, bool replaceText)
        => SelectableEntryHelper.HandleKeyDown(this, keys, replaceText);
}
