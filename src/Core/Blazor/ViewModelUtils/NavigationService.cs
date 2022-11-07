namespace Shipwreck.ViewModelUtils;

public class NavigationService : INavigationService
{
    public NavigationService(NavigationManager navigation)
    {
        NavigationManager = navigation;
    }

    protected NavigationManager NavigationManager { get; }

    public virtual bool IsSupported(object context, NavigationEntry entry)
    => entry?.IsSupported == true;

    public virtual void NavigateTo(object context, NavigationEntry entry)
    {
        if (entry?.Path is string url)
        {
            var au = NavigationManager.ToAbsoluteUri(url);
            if (au.ToString() == NavigationManager.Uri)
            {
                Console.WriteLine("NavigationService.NavigateTo: Suppressed navigating to same url ({0})", au);
                return;
            }
            Console.WriteLine("NavigationService.NavigateTo: {0}", url);
            NavigationManager.NavigateTo(url);
        }
    }
}
