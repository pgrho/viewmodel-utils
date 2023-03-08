namespace Shipwreck.ViewModelUtils;

public interface IFrameworkPageViewModel : IHasInteraction, IRequestFocus, IDisposable, INotifyDataErrorInfo, INotifyPropertyChanged, IHasInteractionService, IHasPageLogger, IHasNavigationService, ICommandViewModelHandler
{
    string Title { get; }
    bool IsBusy { get; }
    bool IsInitializing { get; }
    bool IsInitialized { get; }
    BulkUpdateableCollection<MenuItemViewModel> Menu { get; }
    Task InitializeAsync(); void InvalidateCommands();
}
