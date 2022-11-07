namespace Shipwreck.ViewModelUtils;

public partial class FrameworkPageViewModel : IHasJSRuntime
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

    public ProcessingDisabled DisableProcessing()
        => default;

    partial void PlatformCreateNavigaionService(ref INavigationService s)
        => s = new NavigationService(Page.NavigationManager);
}
