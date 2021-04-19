namespace Shipwreck.ViewModelUtils
{
    public abstract class SelectionCommandViewModelBase : CommandViewModelBase
    {
        protected SelectionCommandViewModelBase(
            string title = null,
            bool isSelected = false,
            string description = null,
            bool isVisible = true,
            bool isEnabled = true,
            BorderStyle style = default,
            int badgeCount = 0)
            : base(title: title,
                  description: description,
                  isVisible: isVisible,
                  isEnabled: isEnabled,
                  style: style,
                  badgeCount: badgeCount)
        {
            _IsSelected = isSelected;
        }

        #region IsSelected

        private bool _IsSelected;

        public bool IsSelected
        {
            get => _IsSelected;
            set
            {
                if (SetProperty(ref _IsSelected, value))
                {
                    Invalidate();
                }
            }
        }

        #endregion IsSelected

        protected override string ComputeIcon() => IsSelected ? "fas fa-check-square" : "far fa-square";
    }
}
