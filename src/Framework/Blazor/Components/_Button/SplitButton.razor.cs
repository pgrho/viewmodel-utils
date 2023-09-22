namespace Shipwreck.ViewModelUtils.Components;

public partial class SplitButton : ListComponentBase<ICommandViewModel, ICommandViewModel>
{
    [Parameter]
    public string BaseClass { get; set; }

    [Parameter]
    public bool? IsVisible { get; set; }

    [Parameter]
    public bool? IsEnabled { get; set; }

    [Parameter]
    public bool? IsActive { get; set; }

    [Parameter]
    public bool ShowIcon { get; set; } = true;

    [Parameter]
    public bool ShowTitle { get; set; } = true;

    [Parameter]
    public bool ShowBadge { get; set; } = true;

    [Parameter]
    public int? TabIndex { get; set; }
}
