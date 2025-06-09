namespace Shipwreck.ViewModelUtils.Components;

public partial class BootstrapToastPresenter : ToastPresenterBase
{
    protected override bool OnItemPropertyChanged(ToastData item, string propertyName)
        => false;

    [Parameter]
    public string DivClass { get; set; }

    [Parameter]
    public string DivStyle { get; set; } = "position:absolute;bottom:0;right:0;padding:1rem 2rem;";

    [Parameter]
    public string ToastStyle { get; set; } = "flex-basis:auto";

    [Parameter]
    public string ToastBodyStyle { get; set; } = "font-size:1.25rem;padding-left:1.5rem;padding-right:1.5rem";
}
