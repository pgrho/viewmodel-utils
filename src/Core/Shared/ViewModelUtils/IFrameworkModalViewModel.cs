using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Shipwreck.ViewModelUtils
{
    public interface IFrameworkModalViewModel : INotifyPropertyChanged, IDisposable
    {
        CommandViewModelBase CancelCommand { get; }

        Task OpenAsync();

        Task CloseAsync();
    }
}
