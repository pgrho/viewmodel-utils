namespace Shipwreck.ViewModelUtils;

public partial interface INavigationService
{
    bool IsSupported(object context, NavigationEntry entry);

    void NavigateTo(object context, NavigationEntry entry);

    bool CanNavigateBack(object context);

    void NavigateBack(object context);
}
