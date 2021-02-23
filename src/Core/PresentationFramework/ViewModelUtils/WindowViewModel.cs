using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using Shipwreck.ViewModelUtils.Validation;

namespace Shipwreck.ViewModelUtils
{
    public abstract partial class WindowViewModel : ValidatableModel, IHasWindow, IDisposable, IRequestFocus
    {
        #region Window

        private Window _Window;

        public Window Window
        {
            get
            {
                if (_Window == null)
                {
                    foreach (var obj in Application.Current.Windows)
                    {
                        if (obj is Window w)
                        {
                            if (w.DataContext == this)
                            {
                                w.Closed += Window_Closed;
                                return _Window = w;
                            }
                        }
                    }
                }
                return _Window;
            }
            protected set
            {
                if (_Window != value)
                {
                    if (_Window != null)
                    {
                        _Window.Closed -= Window_Closed;
                    }

                    _Window = value;
                    RaisePropertyChanged();

                    if (_Window != null)
                    {
                        _Window.Closed -= Window_Closed;
                        _Window.Closed += Window_Closed;
                    }
                }
            }
        }

        private void Window_Closed(object sender, EventArgs e) => Dispose();

        #endregion Window

        #region Interaction

        private IInteractionService _Interaction;

        public IInteractionService Interaction
        {
            get => _Interaction ??= GetInteractionService();
            protected internal set => SetProperty(ref _Interaction, value);
        }

        protected internal bool HasInteraction => _Interaction != null;

        protected virtual IInteractionService GetInteractionService()
            => new InteractionService();

        #endregion Interaction

        #region ApplicationName

        private static string _DefaultApplicationName = _DefaultApplicationName = Assembly.GetEntryAssembly()?.GetName().Name;

        public static string DefaultApplicationName
        {
            get => _DefaultApplicationName;
            set => _DefaultApplicationName = value;
        }

        private string _ApplicationName = DefaultApplicationName;

        public string ApplicationName
        {
            get => _ApplicationName;
            protected set => SetProperty(ref _ApplicationName, value);
        }

        #endregion ApplicationName

        #region Title

        private string _Title;

        public string Title
        {
            get => _Title;
            set => SetProperty(ref _Title, value);
        }

        #endregion Title

        #region IsBusy

        public bool _IsBusy;

        public bool IsBusy
        {
            get => _IsBusy;
            protected set => SetProperty(ref _IsBusy, value);
        }

        protected virtual bool ComputeIsBusy() => false;

        public void SetIsBusy()
            => IsBusy = ComputeIsBusy();

        #endregion IsBusy

        #region IRequestFocus

        public event PropertyChangedEventHandler RequestFocus;

        public virtual void Focus(string propertyName)
            => RequestFocus?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion IRequestFocus

        #region CloseCommand

        private CommandViewModelBase _CloseCommand;

        public CommandViewModelBase CloseCommand
            => _CloseCommand ??= GetCloseCommandBuilder().Build();

        protected virtual CommandBuilderBase GetCloseCommandBuilder()
            => new AsyncCommandBuilder(CloseAsync)
                    .SetTitle("閉じる");

        public Task CloseAsync()
            => Interaction?.CloseModalAsync(this, this);

        #endregion CloseCommand

        #region IDisposable

        protected bool IsDisposed { get; private set; }

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                }

                IsDisposed = true;
            }
        }

        public void Dispose() => Dispose(true);

        #endregion IDisposable
    }
}
