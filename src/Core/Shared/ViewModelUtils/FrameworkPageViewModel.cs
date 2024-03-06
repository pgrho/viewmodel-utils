namespace Shipwreck.ViewModelUtils;

public abstract partial class FrameworkPageViewModel : ValidatableModel, IFrameworkPageViewModel
{
    public static IFrameworkPageViewModelHandler Handler { get; set; }

    public static bool ShouldCaptureContext { get; set; } = TaskHelper.SHOULD_CAPTURE_CONTEXT;

    #region Logger

    private IPageLogger _Logger;

    public IPageLogger Logger
    {
        get => _Logger ??= GetLogger();
        protected set => SetProperty(ref _Logger, value);
    }

    protected virtual IPageLogger GetLogger()
    {
        IPageLogger s = null;
        PlatformGetLogger(ref s);
        return s ?? new ConsolePageLogger(GetType().FullName);
    }

    static partial void PlatformGetLogger(ref IPageLogger service);

    #endregion Logger

    #region Interaction

    private IInteractionService _Interaction;

    public IInteractionService Interaction
    {
        get => _Interaction ??= GetInteractionService();
        protected set => SetProperty(ref _Interaction, value);
    }

    protected virtual IInteractionService GetInteractionService()
    {
        IInteractionService s = null;
        PlatformGetInteractionService(ref s);
        return s;
    }

    static partial void PlatformGetInteractionService(ref IInteractionService service);

    #endregion Interaction

    #region Navigation

    private INavigationService _Navigation;

    public INavigationService Navigation => _Navigation ??= CreateNavigaionService();

    protected virtual INavigationService CreateNavigaionService()
    {
        INavigationService s = null;
        PlatformCreateNavigationService(ref s);
        return s;
    }

    partial void PlatformCreateNavigationService(ref INavigationService s);

    public bool IsSupported(NavigationEntry entry)
        => Navigation?.IsSupported(this, entry) == true;

    public bool NavigateTo(NavigationEntry entry)
    {
        if (IsSupported(entry))
        {
            OnNavigatingTo(entry);
            Navigation.NavigateTo(this, entry);
            return true;
        }
        return false;
    }

    public bool CanNavigateBack()
        => Navigation?.CanNavigateBack(this) == true;

    public bool NavigateBack()
    {
        if (CanNavigateBack())
        {
            Navigation.NavigateBack(this);
            return true;
        }
        return false;
    }

    protected virtual void OnNavigatingTo(NavigationEntry entry)
    { 
    }

    #endregion Navigation

    #region Toast

    protected virtual int MaxToastLogCount => 0;

    protected virtual int MaxRecentToastLogCount => 0;

    private ObservableCollection<ToastLogViewModel> _ToastLogs;

    public ObservableCollection<ToastLogViewModel> ToastLogs
        => _ToastLogs ??= new();

    private ObservableCollection<ToastLogViewModel> _RecentToastLogs;

    public ObservableCollection<ToastLogViewModel> RecentToastLogs
        => _RecentToastLogs ??= new();

    public virtual void EnqueueToastLog(BorderStyle style, string message, string title)
    {
        if (MaxToastLogCount is var toastCount && toastCount > 0)
        {
            if (InvokeRequired)
            {
                InvokeAsync(() => EnqueueToastLog(style, message, title)).ContinueWith(_ => { });
            }
            else
            {
                var vm = new ToastLogViewModel(this, style, message, title);

                while (ToastLogs.Count - toastCount is var diff && diff >= 0)
                {
                    ToastLogs.RemoveAt(diff);
                }

                ToastLogs.Add(vm);

                if (MaxRecentToastLogCount is var rc && rc > 0)
                {
                    RecentToastLogs.Insert(0, vm);

                    while (RecentToastLogs.Count > rc)
                    {
                        RecentToastLogs.RemoveAt(RecentToastLogs.Count - 1);
                    }
                }
            }
        }
    }

    public virtual bool OverridesToast(string message, string title, BorderStyle style) => false;

    #endregion Toast

    #region Title

    private string _Title;

    public string Title
    {
        get => _Title;
        protected set
        {
            if (SetProperty(ref _Title, value))
            {
                OnTitleChanged();
            }
        }
    }

    protected virtual void OnTitleChanged()
    {
    }

    #endregion Title

    #region Flags

    private const byte IS_BUSY = 1;
    private const byte IS_INITIALIZING = 2;
    private const byte IS_INITIALIZED = 4;

    private byte _Flags;

    #region IsBusy

    private int _IsBusyScopeId;
    private readonly HashSet<IsBusyScope> _IsBusyScopes = new();

    public struct IsBusyScope : IDisposable, IEquatable<IsBusyScope>
    {
        private readonly FrameworkPageViewModel _Page;
        private readonly int _Id;
        private readonly Action<bool> _Callback;

        public IsBusyScope(FrameworkPageViewModel page, int id, Action<bool> callback)
        {
            _Page = page;
            _Id = id;
            _Callback = callback;

            callback?.Invoke(true);
        }

        public override bool Equals(object obj)
            => obj is IsBusyScope other && Equals(other);

        public bool Equals(IsBusyScope other)
            => _Page == other._Page && _Id == other._Id;

        public override int GetHashCode()
            => _Id;

        public override string ToString()
            => $"{_Page}#{_Page.GetHashCode()}/{_Id}";

        void IDisposable.Dispose()
        {
            _Callback?.Invoke(false);
            _Page.Dequeue(this);
        }
    }

    public bool IsBusy
    {
        get => (_Flags & IS_BUSY) != 0;
        private set => SetFlagProperty(ref _Flags, IS_BUSY, value);
    }

    protected virtual bool ComputeIsBusy()
    {
        lock (_IsBusyScopes)
        {
            var busy = false;
            for (var i = _ExecutingCommands.Count - 1; i >= 0; i--)
            {
                if (!_ExecutingCommands[i].TryGetTarget(out var c) || !c.IsExecuting)
                {
                    _ExecutingCommands.RemoveAt(i);
                }
                else
                {
                    busy = true;
                }
            }
            return IsInitializing || _IsBusyScopes.Any() || busy;
        }
    }

    protected internal void SetIsBusy()
        => IsBusy = ComputeIsBusy();

    public IsBusyScope EnterBusy(Action<bool> callback = null)
    {
        lock (_IsBusyScopes)
        {
            var r = new IsBusyScope(this, ++_IsBusyScopeId, callback);
            _IsBusyScopes.Add(r);

            SetIsBusy();

            return r;
        }
    }

    private void Dequeue(IsBusyScope scope)
    {
        lock (_IsBusyScopes)
        {
            if (_IsBusyScopes.Remove(scope))
            {
                SetIsBusy();
            }
        }
    }

    #endregion IsBusy

    #region IsInitializing

    public bool IsInitializing
    {
        get => (_Flags & IS_INITIALIZING) != 0;
        private set
        {
            if (SetFlagProperty(ref _Flags, IS_INITIALIZING, value))
            {
                SetIsBusy();
            }
        }
    }

    #endregion IsInitializing

    #region IsInitialized

    public bool IsInitialized
    {
        get => (_Flags & IS_INITIALIZED) != 0;
        private set
        {
            if (SetFlagProperty(ref _Flags, IS_INITIALIZED, value) && IsInitialized)
            {
                OnInitialized();
            }
        }
    }

    protected virtual void OnInitialized()
    {
    }

    #endregion IsInitialized

    #endregion Flags

    #region IsVisible

    private bool _IsVisible;

    public bool IsVisible
    {
        get => _IsVisible;
        set
        {
            if (SetProperty(ref _IsVisible, value))
            {
                if (_IsVisible)
                {
                    OnAppearing().GetHashCode();
                }
                else
                {
                    OnDisappearing().GetHashCode();
                }
            }
        }
    }

    protected virtual async Task OnAppearing()
    {
        try
        {
            if (!IsInitializing && !IsInitialized)
            {
                await InitializeAsync();
            }
            await ResumeConnectionAsync();
        }
        catch (Exception ex)
        {
            LogError("An exception caught while OnAppearing: {0}", ex);
        }
    }

    protected virtual Task OnDisappearing()
    {
        try
        {
            Disconnect();
        }
        catch (Exception ex)
        {
            LogError("An exception caught while OnDisappearing: {0}", ex);
        }

        return Task.CompletedTask;
    }

    #endregion IsVisible

    #region IRealTimeConnection

    private HubConnectionBase _Connection;
    private Task<HubConnectionBase> _ConnectionTask;

    public Task<HubConnectionBase> GetOrCreateConnectionAsync()
        => _ConnectionTask ??= GetOrCreateConnectionAsyncCore();

    private async Task<HubConnectionBase> GetOrCreateConnectionAsyncCore()
    {
        var c = _Connection;
        if (c == null)
        {
            _Connection = c = CreateConnection();
            if (c != null)
            {
                LogInformation("Connecting SignalR");

                await c.StartAsync();
            }
        }
        return c;
    }

    protected virtual HubConnectionBase CreateConnection() => null;

    protected virtual void Disconnect()
    {
        var c = _Connection;
        _Connection = null;
        _ConnectionTask = null;
        BeginDisconnect(c);
    }

    private async void BeginDisconnect(HubConnectionBase c)
    {
        if (c != null)
        {
            LogInformation("Disconnecting SignalR");

            try
            {
                await c.StopAsync();
            }
            catch (Exception ex)
            {
                LogError("An exception caught while HubConnectionBase.StopAsync(): {0}", ex);
            }

            try
            {
                c.Dispose();
            }
            catch (Exception ex)
            {
                LogError("An exception caught while HubConnectionBase.Dispose(): {0}", ex);
            }
        }
    }

    protected virtual Task ResumeConnectionAsync()
        => Task.CompletedTask;

    #endregion IRealTimeConnection

    #region ClearCache

    public static void ClearCache()
    {
        ClearingCache?.Invoke(typeof(FrameworkPageViewModel), EventArgs.Empty);
    }

    public static event EventHandler ClearingCache;

    #endregion ClearCache

    public event PropertyChangedEventHandler RequestFocus;

    public virtual void Focus(string propertyName)
        => RequestFocus?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    public async Task InitializeAsync()
    {
        try
        {
            IsInitializing = true;
            await LoadConfigurationAsync();
            await InitializeDataAsync();
        }
        finally
        {
            IsInitializing = false;
            IsInitialized = true;

            OnTitleChanged();
        }
    }

    #region LoadConfigurationAsync

    private Task _LoadConfigurationTask;

    protected internal Task LoadConfigurationAsync()
    {
        if (_LoadConfigurationTask == null
            || _LoadConfigurationTask.IsCanceled
            || _LoadConfigurationTask.IsFaulted)
        {
            _LoadConfigurationTask = OnLoadConfigurationAsync();
        }
        return _LoadConfigurationTask;
    }

    protected virtual Task OnLoadConfigurationAsync()
        => Task.CompletedTask;

    #endregion LoadConfigurationAsync

    #region InitializeDataAsync

    protected internal virtual Task InitializeDataAsync()
    {
        if (IsAuthorized())
        {
            var ld = LoadInitialDataAsync();
            if (ld != null)
            {
                return ld;
            }
        }
        return Task.CompletedTask;
    }

    protected virtual bool IsAuthorized() => true;

    protected internal virtual Task LoadInitialDataAsync() => null;

    #endregion InitializeDataAsync

    #region OnAfterRenderAsync

    protected internal virtual Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var t2 = OnFirstRenderAsync();
            if (t2 != null)
            {
                return t2;
            }
        }
        return Task.CompletedTask;
    }

    protected internal virtual Task OnFirstRenderAsync() => null;

    #endregion OnAfterRenderAsync

    #region Menu

    private BulkUpdateableCollection<MenuItemViewModel> _Menu;

    public BulkUpdateableCollection<MenuItemViewModel> Menu
        => _Menu ??= new BulkUpdateableCollection<MenuItemViewModel>(CreateMenuItems());

    protected internal virtual IEnumerable<MenuItemViewModel> CreateMenuItems()
        => Enumerable.Empty<MenuItemViewModel>();

    public virtual void InvalidateCommands()
    {
        foreach (var m in Menu)
        {
            m?.Invalidate();
        }
    }

    #endregion Menu

    #region IHasFrameworkPageViewModel

    FrameworkPageViewModel IHasFrameworkPageViewModel.Page => this;

    #endregion IHasFrameworkPageViewModel

    #region ICommandViewModelHandler

    private readonly List<WeakReference<CommandViewModelBase>> _ExecutingCommands = new();

    void ICommandViewModelHandler.OnCommandExecuting(CommandViewModelBase command)
        => OnCommandExecuting(command);

    protected virtual void OnCommandExecuting(CommandViewModelBase command)
    {
        lock (_IsBusyScopes)
        {
            for (var i = _ExecutingCommands.Count - 1; i >= 0; i--)
            {
                if (!_ExecutingCommands[i].TryGetTarget(out var c) || !c.IsExecuting)
                {
                    _ExecutingCommands.RemoveAt(i);
                }
            }
            _ExecutingCommands.Add(new WeakReference<CommandViewModelBase>(command));

            SetIsBusy();
        }
    }

    void ICommandViewModelHandler.OnCommandExecuted(CommandViewModelBase command)
        => OnCommandExecuted(command);

    protected virtual void OnCommandExecuted(CommandViewModelBase command)
    {
        lock (_IsBusyScopes)
        {
            var changed = false;
            for (var i = _ExecutingCommands.Count - 1; i >= 0; i--)
            {
                if (!_ExecutingCommands[i].TryGetTarget(out var c)
                    || !c.IsExecuting
                    || c == command)
                {
                    changed = true;
                    _ExecutingCommands.RemoveAt(i);
                }
            }

            if (changed)
            {
                SetIsBusy();
            }
        }
    }

    #endregion ICommandViewModelHandler

    #region IDisposable

    protected bool IsDisposed { get; set; }

    protected virtual void Dispose(bool disposing)
    {
        if (!IsDisposed)
        {
            try
            {
                _ConnectionTask = null;
                _Connection?.Dispose();
                _Connection = null;
            }
            catch { }
        }
        IsDisposed = true;
    }

#pragma warning disable CA1063 // Implement IDisposable Correctly
    public void Dispose()
#pragma warning restore CA1063 // Implement IDisposable Correctly
            => Dispose(true);

    #endregion IDisposable
}
