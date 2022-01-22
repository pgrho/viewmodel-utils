namespace Shipwreck.ViewModelUtils.Components;

public partial class DateTimePicker : BindableComponentBase
{
    private ElementReference _Element;
    private static int _NewId;
    private string _Id = "DateTimePicker-" + (++_NewId);

    [Parameter]
    public string Id
    {
        get => _Id;
        set => SetProperty(ref _Id, value);
    }

    private bool _ShouldInvoke;

    [Inject]
    public IJSRuntime JS { get; set; }

    #region ClassName

    private string _ClassName = "form-control datetimepicker-input";

    [Parameter]
    public string ClassName
    {
        get => _ClassName;
        set => SetProperty(ref _ClassName, value);
    }

    #endregion ClassName

    #region Placeholder

    private string _Placeholder;

    [Parameter]
    public string Placeholder
    {
        get => _Placeholder;
        set => SetProperty(ref _Placeholder, value);
    }

    #endregion Placeholder

    #region IsEnabled

    private bool _IsEnabled = true;

    [Parameter]
    public bool IsEnabled
    {
        get => _IsEnabled;
        set => SetProperty(ref _IsEnabled, value);
    }

    #endregion IsEnabled

    #region IsReadOnly

    private bool _IsReadOnly;

    [Parameter]
    public bool IsReadOnly
    {
        get => _IsReadOnly;
        set => SetProperty(ref _IsReadOnly, value);
    }

    #endregion IsReadOnly

    #region OnFocus

    private Action _OnFocus;

    [Parameter]
    public Action OnFocus
    {
        get => _OnFocus;
        set => SetProperty(ref _OnFocus, value);
    }

    #endregion OnFocus

    #region OnBlur

    private Action _OnBlur;

    [Parameter]
    public Action OnBlur
    {
        get => _OnBlur;
        set => SetProperty(ref _OnBlur, value);
    }

    #endregion OnBlur

    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object> AdditionalAttributes
    {
        get => _AdditionalAttributes;
        set => SetProperty(ref _AdditionalAttributes, value);
    }
    private IDictionary<string, object> _AdditionalAttributes;

    #region Mode

    private DateTimePickerMode _Mode = DateTimePickerMode.Date;

    [Parameter]
    public DateTimePickerMode Mode
    {
        get => _Mode;
        set
        {
            if (value != _Mode)
            {
                _Mode = value;
                _ShouldInvoke = true;
                StateHasChanged();
            }
        }
    }

    private string ClrFormat
    {
        get
        {
            switch (Mode)
            {
                case DateTimePickerMode.Year:
                    return "yyyy";

                case DateTimePickerMode.Month:
                    return "yyyy-MM";

                case DateTimePickerMode.Hour:
                    return "yyyy-MM-dd HH";

                case DateTimePickerMode.Minute:
                    return "yyyy-MM-dd HH:mm";

                case DateTimePickerMode.Second:
                    return "yyyy-MM-dd HH:mm:ss";
            }
            return "yyyy-MM-dd";
        }
    }

    private string ScriptFormat
    {
        get
        {
            switch (Mode)
            {
                case DateTimePickerMode.Year:
                    return "YYYY";

                case DateTimePickerMode.Month:
                    return "YYYY-MM";

                case DateTimePickerMode.Date:
                    return "YYYY-MM-DD";

                case DateTimePickerMode.Hour:
                    return "YYYY-MM-DD HH";

                case DateTimePickerMode.Minute:
                    return "YYYY-MM-DD HH:mm";
            }

            return "YYYY-MM-DD HH:mm:ss";
        }
    }

    #endregion Mode

    #region Value

    private DateTime? _Value;

    [Parameter]
    public DateTime? Value
    {
        get => _Value;
        set => SetValue(value, true);
    }

    private void SetValue(DateTime? value, bool shouldRender)
    {
        if (value != _Value)
        {
            _Value = value;

            using (Host?.PushPropertyChangedExpectation())
            {
                ValueChanged?.Invoke(_Value);
            }

            if (shouldRender)
            {
                StateHasChanged();
            }
        }
    }

    [Parameter]
    public Action<DateTime?> ValueChanged { get; set; }

    private string FormattedValue
    {
        get => Value?.ToString(ClrFormat);
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                SetValue(null, false);
            }
            else if (DateTime.TryParseExact(value, ClrFormat, null, System.Globalization.DateTimeStyles.None, out var dt)
                    || DateTime.TryParse(value, out dt))
            {
                SetValue(dt, false);
            }
        }
    }

    #endregion Value

    #region UseCurrent

    private bool _UseCurrent;

    [Parameter]
    public bool UseCurrent
    {
        get => _UseCurrent;
        set
        {
            if (_UseCurrent != value)
            {
                _UseCurrent = value;
                _ShouldInvoke = true;
                StateHasChanged();
            }
        }
    }

    #endregion UseCurrent

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        var t = base.OnAfterRenderAsync(firstRender);
        if (t != null)
        {
            await t.ConfigureAwait(false);
        }
        if (firstRender || _ShouldInvoke)
        {
            _ShouldInvoke = true;
            await JS.InvokeVoidAsync(
                "Shipwreck.ViewModelUtils.initDateTimePicker",
                _Element,
                DotNetObjectReference.Create(this),
                FormattedValue,
                ScriptFormat,
                UseCurrent).ConfigureAwait(false);
        }
    }

    public ValueTask FocusAsync(bool selectAll = false)
        => JS.FocusAsync(_Element, selectAll);

    [JSInvokable]
    public void SetValueFromJS(string value)
        => FormattedValue = value;
}
