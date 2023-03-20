namespace Shipwreck.ViewModelUtils;

public partial interface IHasFrameworkPageViewModel : IHasModalPresenter, IHasPopoverPresenter, IHasJSRuntime, Components.IHasBindableComponent
{
    IJSRuntime IHasJSRuntime.JS => Page?.JS;
}
