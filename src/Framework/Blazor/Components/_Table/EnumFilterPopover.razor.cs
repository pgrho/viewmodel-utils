namespace Shipwreck.ViewModelUtils.Components;

public partial class EnumFilterPopover : ComponentBase
{
    private ElementReference _Input;

    [Parameter]
    public ElementReference ReferenceElement { get; set; }

    [Parameter]
    public ElementReference Boundaries { get; set; }

    [Parameter]
    public ModalPresenterBase Presenter { get; set; }

    [Parameter]
    public string Value { get; set; }

    [Parameter]
    public Action<string> ValueChanged { get; set; }

    [Parameter]
    public IReadOnlyList<(string value, string name, bool isSelected)> Options { get; set; }

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
    => presenter.ShowModal(typeof(EnumFilterPopover), new Dictionary<string, object>
    {
        [nameof(Value)] = dataContext.Filter,
        [nameof(ValueChanged)] = (Action<string>)(v => dataContext.Filter = v),
        [nameof(Options)] = (dataContext?.DataContext as IEnumMemberFilter)?.EnumerateOptions().ToArray() ?? [],
        [nameof(Title)] = dataContext.FilterName ?? dataContext.Header,
        [nameof(Description)] = dataContext.FilterDescription,
        [nameof(ReferenceElement)] = reference,
        [nameof(Presenter)] = presenter,
    });
}
