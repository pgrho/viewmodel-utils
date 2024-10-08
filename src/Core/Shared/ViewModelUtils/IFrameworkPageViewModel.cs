﻿namespace Shipwreck.ViewModelUtils;

public interface IFrameworkPageViewModel : IHasInteraction, IRequestFocus, IDisposable, INotifyDataErrorInfo, INotifyPropertyChanged, IHasPageLogger, IHasNavigationService, ICommandViewModelHandler, IHasFrameworkPageViewModel
{
    string Title { get; }
    bool IsBusy { get; }
    bool IsInitializing { get; }
    bool IsInitialized { get; }
    BulkUpdateableCollection<MenuItemViewModel> Menu { get; }
    Task InitializeAsync(); void InvalidateCommands();

    #region ProcessType

    ProcessType ProcessType { get; }

    bool IsWebAssembly { get; }
    bool IsWebServer { get; }
    bool IsBlazor { get; }
    bool IsXamarinForms { get; }
    bool IsWpf { get; }

    #endregion ProcessType
}
