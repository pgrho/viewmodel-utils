namespace Shipwreck.ViewModelUtils;

partial class NavigationEntry
{
    public virtual bool OverrideNavigation(object context) => false;

    public abstract Page GetOrCreatePage(IFrameworkPageViewModelArgs args);
}
