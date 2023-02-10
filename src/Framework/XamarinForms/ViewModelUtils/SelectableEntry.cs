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

    #region IsKeyboardEnabled

    public static readonly BindableProperty IsKeyboardEnabledProperty
        = BindableProperty.Create(
            nameof(IsKeyboardEnabled), typeof(bool), typeof(SelectableEntry),
            defaultValue: true);

    public bool IsKeyboardEnabled
    {
        get => (bool)GetValue(IsKeyboardEnabledProperty);
        set => SetValue(IsKeyboardEnabledProperty, value);
    }

    #endregion IsKeyboardEnabled

    bool IKeyDownHandler.GetIsFocused() => IsFocused;

    public bool HandleKeyDown(string keys, bool replaceText)
        => SelectableEntryHelper.HandleKeyDown(this, keys, replaceText);
}
