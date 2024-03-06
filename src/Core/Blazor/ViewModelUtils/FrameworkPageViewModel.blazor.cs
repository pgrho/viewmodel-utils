namespace Shipwreck.ViewModelUtils;

public partial class FrameworkPageViewModel : IHasJSRuntime, IHasBindableComponent
{
    public struct ProcessingDisabled : IDisposable
    {
        public void Dispose()
        {
        }
    }

    protected FrameworkPageViewModel(FrameworkPageBase page)
    {
        Page = page;
    }

    public FrameworkPageBase Page { get; set; }

    public IJSRuntime JS => Page.JS;

    IBindableComponent IHasBindableComponent.Component => Page;

    public ProcessingDisabled DisableProcessing()
        => default;

    partial void PlatformCreateNavigationService(ref INavigationService s)
        => s = new NavigationService(Page.NavigationManager);
}
