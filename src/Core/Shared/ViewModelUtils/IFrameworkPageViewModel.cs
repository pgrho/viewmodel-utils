using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Shipwreck.ViewModelUtils
{
    public interface IFrameworkPageViewModel : IHasInteraction, IRequestFocus, IDisposable, INotifyDataErrorInfo, INotifyPropertyChanged
    {
        string Title { get; }
        bool IsBusy { get; }
        bool IsInitializing { get; }
        bool IsInitialized { get; }
        BulkUpdateableCollection<MenuItemViewModel> Menu { get; }
        Task InitializeAsync(); void InvalidateCommands();
    }
}
