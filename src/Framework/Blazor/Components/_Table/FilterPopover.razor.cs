namespace Shipwreck.ViewModelUtils.Components;

public partial class FilterPopover : ComponentBase
{
    private ElementReference _Input;

    [Parameter]
    public ElementReference ReferenceElement { get; set; }

    [Parameter]
    public ElementReference Boundaries { get; set; }

    [Parameter]
    public ModalPresenterBase Presenter { get; set; }

    private string InternalValue
    {
        get => Value;
        set
        {
            if (Value != value)
            {
                ValueChanged(value);
            }
        }
    }

    [Parameter]
    public string Value { get; set; }

    [Parameter]
    public Action<string> ValueChanged { get; set; }

    [Parameter]
    public string Title { get; set; }

    [Parameter]
    public string Description { get; set; }

    [Inject]
    public IJSRuntime JS { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await (base.OnAfterRenderAsync(firstRender) ?? Task.CompletedTask);
        if (firstRender)
        {
            await JS.FocusAsync(_Input, true);
        }
    }

    public static void Show(ModalPresenterBase presenter, ElementReference reference, TableHeaderSortableCell dataContext)
    => presenter.ShowModal(typeof(FilterPopover), new Dictionary<string, object>
    {
        [nameof(Value)] = dataContext.Filter,
        [nameof(ValueChanged)] = (Action<string>)(v => dataContext.Filter = v),
        [nameof(Title)] = dataContext.FilterName ?? dataContext.Header,
        [nameof(Description)] = dataContext.FilterDescription,
        [nameof(ReferenceElement)] = reference,
        [nameof(Presenter)] = presenter,
    });
}
