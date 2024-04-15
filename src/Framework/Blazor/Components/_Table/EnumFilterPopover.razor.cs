namespace Shipwreck.ViewModelUtils.Components;

public partial class EnumFilterPopover : ComponentBase
{
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
    public IReadOnlyList<FilterOption> Options { get; set; }

    [Parameter]
    public string Title { get; set; }

    [Parameter]
    public string Description { get; set; }

    public static void Show(ModalPresenterBase presenter, ElementReference reference, string? value, Action<string?> valueChanged, string? title = null, string? description = null, IEnumerable<FilterOption> options = null)
            => presenter.ShowModal(typeof(EnumFilterPopover), new Dictionary<string, object>
            {
                [nameof(Value)] = value,
                [nameof(ValueChanged)] = valueChanged,
                [nameof(Title)] = title,
                [nameof(Description)] = description,
                [nameof(Options)] = options?.ToArray() ?? [],
                [nameof(ReferenceElement)] = reference,
                [nameof(Presenter)] = presenter,
            });
}
