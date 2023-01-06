using Xamarin.KeyboardHelper;

namespace Shipwreck.ViewModelUtils;

public class SelectableEntry : Entry, IKeyDownHandler
{
    public SelectableEntry()
    {
        Effects.Add(new KeyboardEnableEffect());
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

    public bool IsKeyboardEnabled
    {
        get => KeyboardEffect.GetEnableKeyboard(this);
        set => KeyboardEffect.SetEnableKeyboard(this, value);
    }

    bool IKeyDownHandler.GetIsFocused() => IsFocused;

    public bool HandleKeyDown(string keys, bool replaceText)
        => SelectableEntryHelper.HandleKeyDown(this, keys, replaceText);
}
