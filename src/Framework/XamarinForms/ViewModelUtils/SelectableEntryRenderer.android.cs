using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.Text;
using AndroidX.Core.Content;
using Java.Lang;
using Shipwreck.ViewModelUtils;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using VisualElement = Xamarin.Forms.VisualElement;
using Color = Xamarin.Forms.Color;
using Entry = Xamarin.Forms.Entry;
using Android.Text.Method;
using Android.Util;
using Android.Runtime;
using Android.Graphics;
using AndroidX.ConstraintLayout.Widget;

[assembly: ExportRenderer(typeof(SelectableEntry), typeof(SelectableEntryRenderer))]
namespace Shipwreck.ViewModelUtils;

public class SelectableEntryRenderer : ViewRenderer<SelectableEntry, CustomEditText>, ITextWatcher, TextView.IOnEditorActionListener
{
    internal class TextColorSwitcher
    {
        static readonly int[][] s_colorStates = { new[] { global::Android.Resource.Attribute.StateEnabled }, new[] { -global::Android.Resource.Attribute.StateEnabled } };

        readonly ColorStateList _defaultTextColors;
        readonly bool _useLegacyColorManagement;
        Color _currentTextColor;

        public TextColorSwitcher(ColorStateList textColors, bool useLegacyColorManagement = true)
        {
            _defaultTextColors = textColors;
            _useLegacyColorManagement = useLegacyColorManagement;
        }

        public void UpdateTextColor(TextView control, Color color, Action<ColorStateList> setColor = null)
        {
            if (color == _currentTextColor)
            {
                return;
            }

            if (setColor == null)
            {
                setColor = control.SetTextColor;
            }

            _currentTextColor = color;

            if (color.IsDefault)
            {
                setColor(_defaultTextColors);
            }
            else
            {
                if (_useLegacyColorManagement)
                {
                    // Set the new enabled state color, preserving the default disabled state color
                    var defaultDisabledColor = _defaultTextColors.GetColorForState(s_colorStates[1], color.ToAndroid());
                    setColor(new ColorStateList(s_colorStates, new[] { color.ToAndroid().ToArgb(), defaultDisabledColor }));
                }
                else
                {
                    var acolor = color.ToAndroid().ToArgb();
                    setColor(new ColorStateList(s_colorStates, new[] { acolor, acolor }));
                }
            }
        }
    }
    public SelectableEntryRenderer(Context context)
        : base(context)
    {
    }

    protected new SelectableEntry Element => (SelectableEntry)base.Element;

    protected override CustomEditText CreateNativeControl()
        => new CustomEditText(Context);

    protected CustomEditText EditText => Control;
    TextColorSwitcher _hintColorSwitcher;
    TextColorSwitcher _textColorSwitcher;






    bool _disposed;
    ImeAction _currentInputImeFlag;
    IElementController ElementController => Element as IElementController;

    bool _cursorPositionChangePending;
    bool _selectionLengthChangePending;
    bool _nativeSelectionIsUpdating;

    bool TextView.IOnEditorActionListener.OnEditorAction(TextView v, ImeAction actionId, KeyEvent e)
    {
        // Fire Completed and dismiss keyboard for hardware / physical keyboards
        if (actionId == ImeAction.Done || actionId == _currentInputImeFlag || (actionId == ImeAction.ImeNull && e.KeyCode == Keycode.Enter && e.Action == KeyEventActions.Up))
        {
            global::Android.Views.View nextFocus = null;
            if (_currentInputImeFlag == ImeAction.Next)
            {
                nextFocus = FocusSearch(v, FocusSearchDirection.Forward);
            }

            if (nextFocus != null)
            {
                nextFocus.RequestFocus();
                if (!nextFocus.OnCheckIsTextEditor())
                {
                    v.HideKeyboard();
                }
            }
            else
            {
                EditText.ClearFocus();
                v.HideKeyboard();
            }

            ((IEntryController)Element).SendCompleted();
        }

        return true;
    }

    void ITextWatcher.AfterTextChanged(IEditable s)
    {
        OnAfterTextChanged(s);
    }

    void ITextWatcher.BeforeTextChanged(ICharSequence s, int start, int count, int after)
    {
    }

    void ITextWatcher.OnTextChanged(ICharSequence s, int start, int before, int count)
        => TextTransformUtilites.SetPlainText(Element, s?.ToString());

    protected override void OnFocusChangeRequested(object sender, Xamarin.Forms.VisualElement.FocusRequestArgs e)
    {
        if (!e.Focus)
        {
            EditText.HideKeyboard();
        }

        base.OnFocusChangeRequested(sender, e);

        if (e.Focus)
        {
            // Post this to the main looper queue so it doesn't happen until the other focus stuff has resolved
            // Otherwise, ShowKeyboard will be called before this control is truly focused, and we will potentially
            // be displaying the wrong keyboard

            if (Element.IsKeyboardEnabled)
            {
                EditText?.PostShowKeyboard();
            }
        }
    }
    protected override void OnElementChanged(ElementChangedEventArgs<SelectableEntry> e)
    {
        base.OnElementChanged(e);

        if (e.OldElement == null)
        {
            SetNativeControl(CreateNativeControl());

            EditText.AddTextChangedListener(this);
            EditText.SetOnEditorActionListener(this);

            if (EditText is CustomEditText formsEditText)
            {
                formsEditText.OnKeyboardBackPressed += OnKeyboardBackPressed;
                formsEditText.SelectionChanged += SelectionChanged;
            }
        }

        // When we set the control text, it triggers the SelectionChanged event, which updates CursorPosition and SelectionLength;
        // These one-time-use variables will let us initialize a CursorPosition and SelectionLength via ctor/xaml/etc.
        _cursorPositionChangePending = Element.IsSet(Entry.CursorPositionProperty);
        _selectionLengthChangePending = Element.IsSet(Entry.SelectionLengthProperty);

        UpdateIsKeyboardEnabled();
        UpdateSelectAllOnFocus();
        UpdatePlaceholderText();
        UpdateText();
        UpdateInputType();
        UpdateColor();
        UpdateCharacterSpacing();
        UpdateHorizontalTextAlignment();
        UpdateVerticalTextAlignment();
        UpdateFont();
        UpdatePlaceholderColor();
        UpdateMaxLength();
        UpdateImeOptions();
        UpdateReturnType();
        UpdateIsReadOnly();

        if (_cursorPositionChangePending || _selectionLengthChangePending)
        {
            UpdateCursorSelection();
        }

        UpdateClearBtnOnElementChanged();
    }
    protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (this.IsDisposed())
        {
            return;
        }

        Console.WriteLine("OnElementPropertyChanged: " + e.PropertyName);

        switch (e.PropertyName)
        {
            case nameof(Element.Placeholder):
                UpdatePlaceholderText();
                break;

            case nameof(Element.IsPassword):
            case nameof(Element.IsSpellCheckEnabled):
            case nameof(Element.Keyboard):
            case nameof(Element.IsTextPredictionEnabled):
                UpdateInputType();
                break;

            case nameof(Element.Text):
            case nameof(Element.TextTransform):
                UpdateText();
                break;

            case nameof(Element.TextColor):
                UpdateColor();
                break;

            case nameof(Element.FontAttributes):
            case nameof(Element.FontFamily):
            case nameof(Element.FontSize):
                UpdateFont();
                break;

            case nameof(Element.IsKeyboardEnabled):
                UpdateIsKeyboardEnabled();
                break;

            case nameof(Element.SelectAllOnFocus):
                UpdateSelectAllOnFocus();
                break;

            case nameof(Element.IsFocused):
                if (Element.IsFocused)
                {
                    //if (Element.SelectAllOnFocus)
                    //{
                    //    try
                    //    {
                    //        _nativeSelectionIsUpdating = true;
                    //        _cursorPositionChangePending = _selectionLengthChangePending = false;

                    //        var end = Element.Text?.Length ?? 0;
                    //        EditText.SelectAll();

                    //        SetCursorPositionFromRenderer(0);
                    //        SetSelectionLengthFromRenderer(Element.Text?.Length ?? 0);
                    //    }
                    //    catch
                    //    {

                    //    }
                    //    finally
                    //    {
                    //        _nativeSelectionIsUpdating = false;
                    //    }
                    //}
                    if (!Element.IsKeyboardEnabled)
                    {
                        EditText.HideKeyboard();
                    }
                }
                break;
        }

        if (e.PropertyName == Entry.HorizontalTextAlignmentProperty.PropertyName)
        {
            UpdateHorizontalTextAlignment();
        }
        else if (e.PropertyName == Entry.VerticalTextAlignmentProperty.PropertyName)
        {
            UpdateVerticalTextAlignment();
        }
        else if (e.PropertyName == Entry.CharacterSpacingProperty.PropertyName)
        {
            UpdateCharacterSpacing();
        }
        else if (e.PropertyName == Entry.PlaceholderColorProperty.PropertyName)
        {
            UpdatePlaceholderColor();
        }
        else if (e.PropertyName == VisualElement.FlowDirectionProperty.PropertyName)
        {
            UpdateHorizontalTextAlignment();
        }
        else if (e.PropertyName == InputView.MaxLengthProperty.PropertyName)
        {
            UpdateMaxLength();
        }
        else if (e.PropertyName == Xamarin.Forms.PlatformConfiguration.AndroidSpecific.Entry.ImeOptionsProperty.PropertyName)
        {
            UpdateImeOptions();
        }
        else if (e.PropertyName == Entry.ReturnTypeProperty.PropertyName)
        {
            UpdateReturnType();
        }
        else if (e.PropertyName == Entry.SelectionLengthProperty.PropertyName)
        {
            UpdateCursorSelection();
        }
        else if (e.PropertyName == Entry.CursorPositionProperty.PropertyName)
        {
            UpdateCursorSelection();
        }
        else if (e.PropertyName == InputView.IsReadOnlyProperty.PropertyName)
        {
            UpdateIsReadOnly();
        }

        if (e.PropertyName == Entry.ClearButtonVisibilityProperty.PropertyName)
        {
            UpdateClearBtnOnPropertyChanged();
        }

        base.OnElementPropertyChanged(sender, e);
    }

    #region Apply element properties

    protected virtual void UpdateIsReadOnly()
    {
        var isReadOnly = !Element.IsReadOnly;

        EditText.FocusableInTouchMode = isReadOnly;
        EditText.Focusable = isReadOnly;
        EditText.SetCursorVisible(isReadOnly);
    }
    protected virtual void UpdatePlaceholderText()
    {
        if (EditText.Hint == Element.Placeholder)
        {
            return;
        }

        EditText.Hint = Element.Placeholder;
        if (EditText.IsFocused && Element.IsKeyboardEnabled)
        {
            EditText.ShowKeyboard();
        }
    }
    protected virtual void UpdatePlaceholderColor()
    {
        _hintColorSwitcher = _hintColorSwitcher ?? new TextColorSwitcher(EditText.HintTextColors, ((Xamarin.Forms.Entry)Element).UseLegacyColorManagement());
        _hintColorSwitcher.UpdateTextColor(EditText, Element.PlaceholderColor, EditText.SetHintTextColor);
    }

    protected virtual void UpdateColor()
        => UpdateTextColor(Element.TextColor);

    protected virtual void UpdateTextColor(Color color)
    {
        _textColorSwitcher = _textColorSwitcher ?? new TextColorSwitcher(EditText.TextColors, ((Xamarin.Forms.Entry)Element).UseLegacyColorManagement());
        _textColorSwitcher.UpdateTextColor(EditText, color);
    }

    protected virtual void UpdateImeOptions()
    {
        if (Element == null || Control == null)
        {
            return;
        }

        var imeOptions = ((Entry)Element).OnThisPlatform().ImeOptions();
        _currentInputImeFlag = imeOptions.ToAndroidImeOptions();
        EditText.ImeOptions = _currentInputImeFlag;
    }


    protected virtual void UpdateHorizontalTextAlignment()
        => EditText.UpdateTextAlignment(Element.HorizontalTextAlignment, Element.VerticalTextAlignment);


    protected virtual void UpdateVerticalTextAlignment()
        => EditText.UpdateTextAlignment(Element.HorizontalTextAlignment, Element.VerticalTextAlignment);


    protected virtual void UpdateFont()
    {
        EditText.Typeface = Element.ToTypeface();
        EditText.SetTextSize(ComplexUnitType.Sp, (float)Element.FontSize);
    }



    protected virtual void UpdateInputType()
    {
        Entry model = Element;
        var keyboard = model.Keyboard;

        EditText.InputType = keyboard.ToInputType();
        if (!(keyboard is CustomKeyboard))
        {
            if (model.IsSet(InputView.IsSpellCheckEnabledProperty))
            {
                if ((EditText.InputType & InputTypes.TextFlagNoSuggestions) != InputTypes.TextFlagNoSuggestions)
                {
                    if (!model.IsSpellCheckEnabled)
                    {
                        EditText.InputType = EditText.InputType | InputTypes.TextFlagNoSuggestions;
                    }
                }
            }
            if (model.IsSet(Entry.IsTextPredictionEnabledProperty))
            {
                if ((EditText.InputType & InputTypes.TextFlagNoSuggestions) != InputTypes.TextFlagNoSuggestions)
                {
                    if (!model.IsTextPredictionEnabled)
                    {
                        EditText.InputType = EditText.InputType | InputTypes.TextFlagNoSuggestions;
                    }
                }
            }
        }

        if (keyboard == Keyboard.Numeric)
        {
            EditText.KeyListener = GetDigitsKeyListener(EditText.InputType);
        }

        if (model.IsPassword && ((EditText.InputType & InputTypes.ClassText) == InputTypes.ClassText))
        {
            EditText.InputType = EditText.InputType | InputTypes.TextVariationPassword;
        }

        if (model.IsPassword && ((EditText.InputType & InputTypes.ClassNumber) == InputTypes.ClassNumber))
        {
            EditText.InputType = EditText.InputType | InputTypes.NumberVariationPassword;
        }

        UpdateFont();
    }



    protected virtual void UpdateCharacterSpacing()
    {
        if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
        {
            EditText.LetterSpacing = Element.CharacterSpacing.ToEm();
        }
    }


    protected virtual void UpdateReturnType()
    {
        if (Control == null || Element == null)
        {
            return;
        }

        EditText.ImeOptions = Element.ReturnType.ToAndroidImeAction();
        _currentInputImeFlag = EditText.ImeOptions;
    }
    protected virtual void UpdateCursorSelection()
    {
        if (_nativeSelectionIsUpdating || Control == null || Element == null || EditText == null)
        {
            return;
        }

        if (!Element.IsReadOnly && EditText.RequestFocus())
        {
            try
            {
                var start = GetSelectionStart();
                var end = GetSelectionEnd(start);

                EditText.SetSelection(start, end);
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to set Control.Selection from CursorPosition/SelectionLength: {ex}");
            }
            finally
            {
                _cursorPositionChangePending = _selectionLengthChangePending = false;
            }
        }
    }

    protected virtual void UpdateText()
    {
        var text = Element.UpdateFormsText(Element.Text, Element.TextTransform);

        if (EditText.Text == text)
        {
            return;
        }

        EditText.Text = text;
        if (EditText.IsFocused)
        {
            EditText.SetSelection(text.Length);

            if (Element.IsKeyboardEnabled)
            {
                EditText.ShowKeyboard();
            }
        }
    }

    protected virtual void UpdateIsKeyboardEnabled()
    {
        if (Element is SelectableEntry e && Control is CustomEditText c)
        {
            c.ShowSoftInputOnFocus = e.IsKeyboardEnabled;

            if (c.IsFocused)
            {
                if (e.IsKeyboardEnabled)
                {
                    c.ShowKeyboard();
                }
                else
                {
                    c.HideKeyboard();
                }
            }
        }
    }

    protected virtual void UpdateSelectAllOnFocus()
    {
        if (Element is SelectableEntry e && Control is CustomEditText c)
        {
            c.SetSelectAllOnFocus(e.SelectAllOnFocus);
        }
    }

    protected virtual void UpdateMaxLength()
    {
        var currentFilters = new List<IInputFilter>(EditText?.GetFilters() ?? new IInputFilter[0]);

        for (var i = 0; i < currentFilters.Count; i++)
        {
            if (currentFilters[i] is InputFilterLengthFilter)
            {
                currentFilters.RemoveAt(i);
                break;
            }
        }

        currentFilters.Add(new InputFilterLengthFilter(Element.MaxLength));

        EditText?.SetFilters(currentFilters.ToArray());

        var currentControlText = EditText?.Text;

        if (currentControlText.Length > Element.MaxLength)
        {
            EditText.Text = currentControlText.Substring(0, Element.MaxLength);
        }
    }


    #endregion


    protected override void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;

        if (disposing)
        {
            if (EditText != null)
            {
                EditText.RemoveTextChangedListener(this);
                EditText.SetOnEditorActionListener(null);

                if (EditText is CustomEditText formsEditContext)
                {
                    formsEditContext.OnKeyboardBackPressed -= OnKeyboardBackPressed;
                    formsEditContext.SelectionChanged -= SelectionChanged;

                    ListenForCloseBtnTouch(false);
                }
            }

            _clearBtn = null;
        }

        base.Dispose(disposing);
    }

    protected virtual NumberKeyListener GetDigitsKeyListener(InputTypes inputTypes)
    {
        // Override this in a custom renderer to use a different NumberKeyListener
        // or to filter out input types you don't want to allow
        // (e.g., inputTypes &= ~InputTypes.NumberFlagSigned to disallow the sign)
        return LocalizedDigitsKeyListener.Create(inputTypes);
    }




    void OnKeyboardBackPressed(object sender, EventArgs eventArgs)
    {
        Control?.ClearFocus();
    }

    void SelectionChanged(object sender, Xamarin.Forms.Platform.Android.SelectionChangedEventArgs e)
    {
        if (_nativeSelectionIsUpdating || Control == null || Element == null)
        {
            return;
        }

        var cursorPosition = Element.CursorPosition;
        var selectionStart = EditText.SelectionStart;

        if (!_cursorPositionChangePending)
        {
            var start = cursorPosition;

            if (selectionStart != start)
            {
                SetCursorPositionFromRenderer(selectionStart);
            }
        }

        if (!_selectionLengthChangePending)
        {
            var elementSelectionLength = System.Math.Min(EditText.Text.Length - cursorPosition, Element.SelectionLength);

            var controlSelectionLength = EditText.SelectionEnd - selectionStart;
            if (controlSelectionLength != elementSelectionLength)
            {
                SetSelectionLengthFromRenderer(controlSelectionLength);
            }
        }
    }



    int GetSelectionEnd(int start)
    {
        var end = start;
        var selectionLength = Element.SelectionLength;

        if (Element.IsSet(Entry.SelectionLengthProperty))
        {
            end = System.Math.Max(start, System.Math.Min(EditText.Length(), start + selectionLength));
        }

        var newSelectionLength = System.Math.Max(0, end - start);
        if (newSelectionLength != selectionLength)
        {
            SetSelectionLengthFromRenderer(newSelectionLength);
        }

        return end;
    }

    int GetSelectionStart()
    {
        var start = EditText.Length();
        var cursorPosition = Element.CursorPosition;

        if (Element.IsSet(Entry.CursorPositionProperty))
        {
            start = System.Math.Min(EditText.Text.Length, cursorPosition);
        }

        if (start != cursorPosition)
        {
            SetCursorPositionFromRenderer(start);
        }

        return start;
    }

    void SetCursorPositionFromRenderer(int start)
    {
        try
        {
            _nativeSelectionIsUpdating = true;
            ElementController?.SetValueFromRenderer(Entry.CursorPositionProperty, start);
        }
        catch (System.Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to set CursorPosition from renderer: {ex}");
        }
        finally
        {
            _nativeSelectionIsUpdating = false;
        }
    }

    void SetSelectionLengthFromRenderer(int selectionLength)
    {
        try
        {
            _nativeSelectionIsUpdating = true;
            ElementController?.SetValueFromRenderer(Entry.SelectionLengthProperty, selectionLength);
        }
        catch (System.Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to set SelectionLength from renderer: {ex}");
        }
        finally
        {
            _nativeSelectionIsUpdating = false;
        }
    }




    Drawable _clearBtn;

    protected override void OnFocusChanged(bool gainFocus, [GeneratedEnum] FocusSearchDirection direction, Android.Graphics.Rect previouslyFocusedRect)
    {
        if (!Element.IsKeyboardEnabled)
        {
            EditText.HideKeyboard();
        }

        base.OnFocusChanged(gainFocus, direction, previouslyFocusedRect);
        UpdateClearBtnOnFocusChanged(gainFocus);
    }

    void OnAfterTextChanged(IEditable s)
    {
        if (Control.IsFocused)
        {
            UpdateClearBtnOnTyping();
        }
    }
    void EditTextTouched(object sender, TouchEventArgs e)
    {
        e.Handled = false;
        var me = e.Event;

        var rBounds = _clearBtn?.Bounds;
        if (rBounds != null)
        {
            var x = me.GetX();
            var y = me.GetY();
            if (me.Action == MotionEventActions.Up
                && ((x >= (EditText.Right - rBounds.Width())
                && x <= (EditText.Right - EditText.PaddingRight)
                && y >= EditText.PaddingTop
                && y <= (EditText.Height - EditText.PaddingBottom)
                && (Element as IVisualElementController).EffectiveFlowDirection.IsLeftToRight())
                || (x >= (EditText.Left + EditText.PaddingLeft)
                && x <= (EditText.Left + rBounds.Width())
                && y >= EditText.PaddingTop
                && y <= (EditText.Height - EditText.PaddingBottom)
                && (Element as IVisualElementController).EffectiveFlowDirection.IsRightToLeft())))
            {
                EditText.Text = null;
                e.Handled = true;
            }
        }
    }


    void UpdateClearBtnOnElementChanged()
    {
        var showClearBtn = Element.ClearButtonVisibility == ClearButtonVisibility.WhileEditing;
        if (showClearBtn && Element.IsFocused)
        {
            UpdateClearBtn(true);
            ListenForCloseBtnTouch(true);
        }
    }

    void UpdateClearBtnOnPropertyChanged()
    {
        var isFocused = Element.IsFocused;
        if (isFocused)
        {
            var showClearBtn = Element.ClearButtonVisibility == ClearButtonVisibility.WhileEditing;
            UpdateClearBtn(showClearBtn);

            if (!showClearBtn && isFocused)
            {
                ListenForCloseBtnTouch(false);
            }
        }
    }

    void UpdateClearBtnOnFocusChanged(bool isFocused)
    {
        if (Element.ClearButtonVisibility == ClearButtonVisibility.WhileEditing)
        {
            UpdateClearBtn(isFocused);
            ListenForCloseBtnTouch(isFocused);
        }
    }

    void UpdateClearBtnOnTyping()
    {
        if (Element.ClearButtonVisibility == ClearButtonVisibility.WhileEditing)
        {
            UpdateClearBtn(true);
        }
    }

    void UpdateClearBtn(bool showClearButton)
    {
        var d = showClearButton && (Element.Text?.Length > 0) ? GetCloseButtonDrawable() : null;
        if ((Element as IVisualElementController).EffectiveFlowDirection.IsRightToLeft())
        {
            EditText.SetCompoundDrawablesWithIntrinsicBounds(d, null, null, null);
        }
        else
        {
            EditText.SetCompoundDrawablesWithIntrinsicBounds(null, null, d, null);
        }
        _clearBtn = d;
    }

    void ListenForCloseBtnTouch(bool listen)
    {
        if (listen)
        {
            EditText.Touch += EditTextTouched;
        }
        else
        {
            EditText.Touch -= EditTextTouched;
        }
    }

    protected virtual Drawable GetCloseButtonDrawable()
        => ContextCompat.GetDrawable(Context, Xamarin.Forms.Platform.Android.Resource.Drawable.abc_ic_clear_material);
}
