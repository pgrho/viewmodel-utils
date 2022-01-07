using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Shipwreck.ViewModelUtils.Client;
using Shipwreck.ViewModelUtils.Validation;

namespace Shipwreck.ViewModelUtils
{
    public abstract partial class FrameworkPageViewModel : ValidatableModel, IFrameworkPageViewModel
    {
        public static bool ShouldCaptureContext => TaskHelper.SHOULD_CAPTURE_CONTEXT;

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

        public bool IsBusy
        {
            get => (_Flags & IS_BUSY) != 0;
            private set => SetFlagProperty(ref _Flags, IS_BUSY, value);
        }

        protected virtual bool ComputeIsBusy()
            => IsInitializing;

        protected internal void SetIsBusy()
            => IsBusy = ComputeIsBusy();

        #endregion IsBusy

        #region IsInitializing

        public bool IsInitializing
        {
            get => (_Flags & IS_INITIALIZING) != 0;
            private set
            {
                if (SetFlagProperty(ref _Flags, IS_INITIALIZING, value))
                {
                    ComputeIsBusy();
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
                    await InitializeAsync().ConfigureAwait();
                }
                await ResumeConnectionAsync().ConfigureAwait();
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

                    await c.StartAsync().ConfigureAwait();
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
                    await c.StopAsync().ConfigureAwait(false);
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
                await LoadConfigurationAsync().ConfigureAwait();
                await InitializeDataAsync().ConfigureAwait();
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

        #region IDisposable

        protected bool IsDisposed { get; set; }

        protected virtual void Dispose(bool disposing)
        {
            IsDisposed = true;
        }

#pragma warning disable CA1063 // Implement IDisposable Correctly

        public void Dispose()
#pragma warning restore CA1063 // Implement IDisposable Correctly
            => Dispose(true);

        #endregion IDisposable
    }
}
