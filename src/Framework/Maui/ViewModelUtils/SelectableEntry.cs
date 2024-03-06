using Shipwreck.BootstrapControls.Platforms.Android;
using Shipwreck.BootstrapControls;

namespace Shipwreck.ViewModelUtils;

public class SelectableEntry : Entry, IKeyDownHandler
{
    public SelectableEntry()
    {
        ReturnType = ReturnType.Done;
    }

    #region SelectAllOnFocus

    public static readonly BindableProperty SelectAllOnFocusProperty
        = BindableProperty.Create(
            nameof(SelectAllOnFocus), typeof(bool), typeof(SelectableEntry),
            defaultValue: true);

    public bool SelectAllOnFocus
    {
        get => (bool)GetValue(SelectAllOnFocusProperty);
        set => SetValue(SelectAllOnFocusProperty, value);
    }

    #endregion SelectAllOnFocus

    #region IsSoftwareKeyboardEnabled

    public static readonly BindableProperty IsSoftwareKeyboardEnabledProperty
        = BindableProperty.Create(
            nameof(IsSoftwareKeyboardEnabled), typeof(bool), typeof(SelectableEntry),
            defaultValue: true);

    public bool IsSoftwareKeyboardEnabled
    {
        get => (bool)GetValue(IsSoftwareKeyboardEnabledProperty);
        set => SetValue(IsSoftwareKeyboardEnabledProperty, value);
    }

    #endregion IsSoftwareKeyboardEnabled

    bool IKeyDownHandler.GetIsFocused() => IsFocused;

    public bool HandleKeyDown(string keys, bool replaceText)
        => SelectableEntryHelper.HandleKeyDown(this, keys, replaceText);
}
