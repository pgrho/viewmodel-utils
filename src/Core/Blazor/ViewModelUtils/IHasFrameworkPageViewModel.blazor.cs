using Microsoft.JSInterop;

namespace Shipwreck.ViewModelUtils
{
    public partial interface IHasFrameworkPageViewModel : IHasModalPresenter, IHasPopoverPresenter, IHasJSRuntime
    {
        IJSRuntime IHasJSRuntime.JS => Page?.JS;
    }
}
