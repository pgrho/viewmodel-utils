namespace Shipwreck.ViewModelUtils.Components
{
    public partial class SplitButton : ListComponentBase<ICommandViewModel, ICommandViewModel>
    {
        #region BaseClass

        private string _BaseClass;

        [Parameter]
        public string BaseClass
        {
            get => _BaseClass;
            set => SetProperty(ref _BaseClass, value);
        }

        #endregion BaseClass

        #region IsVisible

        private bool? _IsVisible;

        [Parameter]
        public bool? IsVisible
        {
            get => _IsVisible;
            set => SetProperty(ref _IsVisible, value);
        }

        #endregion IsVisible

        #region IsEnabled

        private bool? _IsEnabled;

        [Parameter]
        public bool? IsEnabled
        {
            get => _IsEnabled;
            set => SetProperty(ref _IsEnabled, value);
        }

        #endregion IsEnabled

        #region IsActive

        private bool? _IsActive;

        [Parameter]
        public bool? IsActive
        {
            get => _IsActive;
            set => SetProperty(ref _IsActive, value);
        }

        #endregion IsActive

        #region ShowIcon

        private bool _ShowIcon = true;

        [Parameter]
        public bool ShowIcon
        {
            get => _ShowIcon;
            set => SetProperty(ref _ShowIcon, value);
        }

        #endregion ShowIcon

        #region ShowTitle

        private bool _ShowTitle = true;

        [Parameter]
        public bool ShowTitle
        {
            get => _ShowTitle;
            set => SetProperty(ref _ShowTitle, value);
        }

        #endregion ShowTitle

        #region ShowBadge

        private bool _ShowBadge = true;

        [Parameter]
        public bool ShowBadge
        {
            get => _ShowBadge;
            set => SetProperty(ref _ShowBadge, value);
        }

        #endregion ShowBadge
    }
}
