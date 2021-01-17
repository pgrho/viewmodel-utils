using System;
using System.Windows.Input;

namespace Shipwreck.ViewModelUtils
{
    public abstract partial class CommandViewModelBase : ObservableModel, ICommandViewModel, IDisposable, ICommand
    {
        protected CommandViewModelBase(
            string title = null,
            string description = null,
            bool isVisible = true,
            bool isEnabled = true,
            string icon = null,
            BorderStyle style = default,
            int badgeCount = 0,
            string href = null)
        {
            _Title = title;
            _Description = description;
            _IsVisible = isVisible;
            _IsEnabled = isEnabled;
            _Href = href;
            _Icon = icon;
            _Style = style;
            _BadgeCount = badgeCount;
        }

        #region Title

        private string _Title;

        public string Title
        {
            get => _Title;
            protected set => SetProperty(ref _Title, value);
        }

        protected virtual string ComputeTitle() => null;

        #endregion Title

        #region Description

        private string _Description;

        public string Description
        {
            get => _Description;
            protected set => SetProperty(ref _Description, value);
        }

        protected virtual string ComputeDescription() => null;

        #endregion Description

        #region IsVisible

        private bool _IsVisible = true;

        public bool IsVisible
        {
            get => _IsVisible;
            set
            {
                var ce = IsEnabled && IsVisible;
                if (SetProperty(ref _IsVisible, value))
                {
                    if (ce != (IsEnabled && IsVisible))
                    {
                        _CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        }

        protected virtual bool? ComputeIsVisible() => null;

        #endregion IsVisible

        #region IsEnabled

        private bool _IsEnabled = true;

        public bool IsEnabled
        {
            get => _IsEnabled;
            protected set
            {
                var ce = IsEnabled && IsVisible;
                if (SetProperty(ref _IsEnabled, value))
                {
                    Invalidate();
                    if (ce != (IsEnabled && IsVisible))
                    {
                        _CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        }

        protected virtual bool? ComputeIsEnabled() => null;

        #endregion IsEnabled

        #region Href

        private string _Href;

        public string Href
        {
            get => _Href;
            protected set => SetProperty(ref _Href, value);
        }

        protected virtual string ComputeHref() => null;

        #endregion Href

        #region IsExecuting

        private bool _IsExecuting;

        public bool IsExecuting
        {
            get => _IsExecuting;
            set
            {
                if (SetProperty(ref _IsExecuting, value))
                {
                    Invalidate();
                }
            }
        }

        #endregion IsExecuting

        #region Icon

        private string _Icon;

        public string Icon
        {
            get => _Icon;
            protected set => SetProperty(ref _Icon, value);
        }

        protected virtual string ComputeIcon() => null;

        #endregion Icon

        #region Style

        private BorderStyle _Style;

        public BorderStyle Style
        {
            get => _Style;
            protected set => SetProperty(ref _Style, value);
        }

        protected virtual BorderStyle? ComputeStyle() => null;

        #endregion Style

        #region BadgeCount

        private int _BadgeCount;

        public int BadgeCount
        {
            get => _BadgeCount;
            protected set => SetProperty(ref _BadgeCount, Math.Max(0, value));
        }

        protected virtual int? ComputeBadgeCount() => null;

        #endregion BadgeCount

        public virtual void Invalidate()
        {
            Title = ComputeTitle() ?? _Title;
            Description = ComputeDescription() ?? _Description;
            IsVisible = ComputeIsVisible() ?? _IsVisible;
            IsEnabled = ComputeIsEnabled() ?? _IsEnabled;
            Href = ComputeHref() ?? _Href;
            Icon = ComputeIcon() ?? _Icon;
            Style = ComputeStyle() ?? _Style;
            BadgeCount = ComputeBadgeCount() ?? _BadgeCount;
        }

        public abstract void Execute();

        #region ICommand

        private EventHandler _CanExecuteChanged;

        event EventHandler ICommand.CanExecuteChanged
        {
            add => _CanExecuteChanged += value;
            remove => _CanExecuteChanged -= value;
        }

        bool ICommand.CanExecute(object parameter) => IsEnabled && IsVisible;

        void ICommand.Execute(object parameter) => Execute();

        #endregion ICommand

        #region IDisposable

        protected bool IsDisposed { get; set; }

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                IsDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable
    }
}
