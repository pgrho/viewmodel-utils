﻿namespace Shipwreck.ViewModelUtils.Components;

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

    public static void Show(ModalPresenterBase presenter, ElementReference reference, string? value, Action<string?> valueChanged, string? title = null, string? description = null)
        => presenter.ShowModal(typeof(FilterPopover), new Dictionary<string, object>
        {
            [nameof(Value)] = value,
            [nameof(ValueChanged)] = valueChanged,
            [nameof(Title)] = title,
            [nameof(Description)] = description,
            [nameof(ReferenceElement)] = reference,
            [nameof(Presenter)] = presenter,
        });
}
