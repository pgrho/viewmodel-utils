namespace Shipwreck.ViewModelUtils.Components;

public partial class DateTimePicker : BindableComponentBase
{
    private ElementReference _Element;
    private static int _NewId;

    [Parameter]
    public string Id { get; set; } = "DateTimePicker-" + (++_NewId);


    [Inject]
    public IJSRuntime JS { get; set; }

    private IJSInProcessRuntime IJS => JS as IJSInProcessRuntime;

    #region Value

    [Parameter]
    public DateTime? Value { get; set; }

    [Parameter]
    public DateTime NonNullValue { get; set; }

    [Parameter]
    public Action<DateTime?> ValueChanged { get; set; }

    [Parameter]
    public Action<DateTime> NonNullValueChanged { get; set; }

    #endregion Value

    [Parameter]
    public string SpanStyle { get; set; }

    [Parameter]
    public string ClassName { get; set; } = "form-control";

    [Parameter]
    public DateTimePickerMode Mode { get; set; } = DateTimePickerMode.Date;

    [Parameter]
    public DateTime? MinimumDate { get; set; }

    [Parameter]
    public DateTime? MaximumDate { get; set; }

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

    [Parameter]
    public Func<DateTime, DateTime, Task<IReadOnlyDictionary<DateTime, string>>> MonthDataProvider { get; set; }

    public void Focus(bool selectAll = false)
        => IJS.Focus(_Element, selectAll);

    public ValueTask FocusAsync(bool selectAll = false)
        => JS.FocusAsync(_Element, selectAll);
}
