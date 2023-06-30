namespace Shipwreck.ViewModelUtils;

public partial interface IFrameworkModalViewModel : INotifyPropertyChanged, IDisposable, IHasInteractionService, IHasPageLogger
{
    CommandViewModelBase CancelCommand { get; }

    Task OpenAsync();

    Task CloseAsync();
}
