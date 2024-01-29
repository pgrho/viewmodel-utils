namespace Shipwreck.ViewModelUtils;

public partial interface IFrameworkModalViewModel : INotifyPropertyChanged, IDisposable, IHasInteractionService, IHasPageLogger, IRequestFocus
{
    CommandViewModelBase CancelCommand { get; }

    Task OpenAsync();

    Task CloseAsync();
}
