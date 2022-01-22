namespace Shipwreck.ViewModelUtils.Components
{
    public partial class CommandButton : BindableComponentBase<ICommandViewModel>
    {
        #region BaseClass

        private string _BaseClass = "btn";

        [Parameter]
        public string BaseClass
        {
            get => _BaseClass;
            set => SetProperty(ref _BaseClass, value);
        }

        #endregion BaseClass

        #region CommandStyle

        private BorderStyle? _CommandStyle;

        [Parameter]
        public BorderStyle? CommandStyle
        {
            get => _CommandStyle;
            set => SetProperty(ref _CommandStyle, value);
        }

        #endregion CommandStyle

        #region Icon

        private string _Icon;

        [Parameter]
        public string Icon
        {
            get => _Icon;
            set => SetProperty(ref _Icon, value);
        }

        #endregion Icon

        #region Title

        private string _Title;

        [Parameter]
        public string Title
        {
            get => _Title;
            set => SetProperty(ref _Title, value);
        }

        #endregion Title

        #region BadgeCount

        private int? _BadgeCount;

        [Parameter]
        public int? BadgeCount
        {
            get => _BadgeCount;
            set => SetProperty(ref _BadgeCount, value);
        }

        #endregion BadgeCount

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

        [Parameter(CaptureUnmatchedValues = true)]
        public IDictionary<string, object> AdditionalAttributes
        {
            get => _Attributes;
            set => SetProperty(ref _Attributes, value);
        }
        private IDictionary<string, object> _Attributes;

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

        #region BadgeStyle

        private BorderStyle _BadgeStyle = BorderStyle.Danger;

        [Parameter]
        public BorderStyle BadgeStyle
        {
            get => _BadgeStyle;
            set => SetProperty(ref _BadgeStyle, value);
        }

        #endregion BadgeStyle
    }
}
