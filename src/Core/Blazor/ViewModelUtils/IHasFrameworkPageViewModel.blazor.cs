namespace Shipwreck.ViewModelUtils;

public partial interface IHasFrameworkPageViewModel : IHasJSRuntime, Components.IHasBindableComponent
{
    IJSRuntime IHasJSRuntime.JS => Page?.JS;
}
