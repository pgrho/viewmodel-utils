using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.ViewModelUtils
{
    public abstract partial class FrameworkPageViewModel : ObservableModel, IRequestFocus, IDisposable
    {
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

        protected virtual void OnTitleChanged() { }

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
            private set => SetFlagProperty(ref _Flags, IS_INITIALIZED, value);
        }

        #endregion IsInitialized

        #endregion Flags

        public event PropertyChangedEventHandler RequestFocus;

        public virtual void Focus(string propertyName)
            => RequestFocus?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public async Task InitializeAsync()
        {
            try
            {
                IsInitializing = true;
                await LoadConfigurationAsync().ConfigureAwait(false);
                await InitializeDataAsync().ConfigureAwait(false);
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
