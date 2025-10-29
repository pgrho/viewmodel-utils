namespace Shipwreck.ViewModelUtils.Components;

public partial class CommandButton : BindableComponentBase<ICommandViewModel>
{
    private ElementReference _Element;
    [Parameter]
    public string BaseClass { get; set; } = "btn";

    [Parameter]
    public BorderStyle? CommandStyle { get; set; }

    [Parameter]
    public string Icon { get; set; }

    [Parameter]
    public string Title { get; set; }

    [Parameter]
    public int? BadgeCount { get; set; }

    [Parameter]
    public int? TabIndex { get; set; }

    [Parameter]
    public bool? IsVisible { get; set; }

    [Parameter]
    public bool? IsEnabled { get; set; }

    [Parameter]
    public bool? IsActive { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object> AdditionalAttributes { get; set; }

    [Parameter]
    public bool ShowIcon { get; set; } = true;

    [Parameter]
    public bool ShowTitle { get; set; } = true;

    [Parameter]
    public bool ShowBadge { get; set; } = true;

    [Parameter]
    public BorderStyle BadgeStyle { get; set; } = BorderStyle.Danger;

    [Parameter]
    public RenderFragment ChildContent { get; set; }

    [Inject]
    public IJSRuntime JS { get; set; }

    public ValueTask FocusAsync()
        => JS.FocusAsync(_Element, false);
}
