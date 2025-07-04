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
            var nm = (context as IHasNavigationManager)?.NavigationManager
                ?? (context as IHasFrameworkPageViewModel)?.Page?.NavigationManager
                ?? NavigationManager;
            var au = nm.ToAbsoluteUri(url);
            if (au.ToString() == nm.Uri)
            {
                Console.WriteLine("NavigationService.NavigateTo: Suppressed navigating to same url ({0})", au);
                return;
            }
            Console.WriteLine("NavigationService.NavigateTo: {0}", url);
            nm.NavigateTo(url);
        }
    }

    public virtual bool CanNavigateBack(object context) => false;

    public virtual void NavigateBack(object context)
    {
    }
}
