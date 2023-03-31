namespace Shipwreck.ViewModelUtils.Components;

public partial class DateTimePicker : BindableComponentBase
{
    private ElementReference _Element;
    private static int _NewId; 

    [Parameter]
    public string Id { get; set; } = "DateTimePicker-" + (++_NewId);

    private bool _ShouldInvoke;

    [Inject]
    public IJSRuntime JS { get; set; }

    

    [Parameter]
    public string ClassName { get; set; }= "form-control datetimepicker-input";
     
     
    [Parameter]
    public string Placeholder { get; set; }
     

    [Parameter]
    public bool IsEnabled { get; set; } = true;

 

    [Parameter]
    public bool IsReadOnly { get; set; }

  

    [Parameter]
    public bool IsRequired { get; set; }

  

    [Parameter]
    public Action OnFocus { get; set; }

 

    [Parameter]
    public Action OnBlur { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object> AdditionalAttributes { get; set; }

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
            await t.ConfigureAwait();
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
                UseCurrent).ConfigureAwait();
        }
    }

    public ValueTask FocusAsync(bool selectAll = false)
        => JS.FocusAsync(_Element, selectAll);

    [JSInvokable]
    public void SetValueFromJS(string value)
        => FormattedValue = value;
}
