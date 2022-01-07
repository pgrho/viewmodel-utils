namespace Shipwreck.ViewModelUtils;

public interface IFrameworkModalViewModel : INotifyPropertyChanged, IDisposable, IHasInteractionService, IHasPageLogger
{
    CommandViewModelBase CancelCommand { get; }

    Task OpenAsync();

    Task CloseAsync();
}
