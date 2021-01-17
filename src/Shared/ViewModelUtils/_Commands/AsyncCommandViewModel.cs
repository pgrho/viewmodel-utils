using System;
using System.Threading.Tasks;

namespace Shipwreck.ViewModelUtils
{
    internal sealed partial class AsyncCommandViewModel : CommandViewModelBase
    {
        #region Instance Members

        private readonly Func<Task> _Execute;
        private readonly Func<string> _TitleGetter = null;
        private readonly Func<string> _DescriptionGetter = null;
        private readonly Func<bool> _IsVisibleGetter = null;
        private readonly Func<bool> _IsEnabledGetter = null;
        private readonly Func<string> _IconGetter = null;
        private readonly Func<BorderStyle> _TypeGetter = null;
        private readonly Func<int> _BadgeCountGetter = null;

        public AsyncCommandViewModel(
            Func<Task> execute
            , string title = null, Func<string> titleGetter = null
            , string description = null, Func<string> descriptionGetter = null
            , bool isVisible = true, Func<bool> isVisibleGetter = null
            , bool isEnabled = true, Func<bool> isEnabledGetter = null
            , string icon = null, Func<string> iconGetter = null
            , BorderStyle style = default, Func<BorderStyle> styleGetter = null
            , int badgeCount = 0, Func<int> badgeCountGetter = null
            )
            : base(
                  title: titleGetter?.Invoke() ?? title
                  , description: descriptionGetter?.Invoke() ?? description
                  , isVisible: isVisibleGetter?.Invoke() ?? isVisible
                  , isEnabled: isEnabledGetter?.Invoke() ?? isEnabled
                  , icon: iconGetter?.Invoke() ?? icon
                  , style: styleGetter?.Invoke() ?? style
                  , badgeCount: badgeCountGetter?.Invoke() ?? badgeCount)
        {
            _Execute = execute;
            _TitleGetter = titleGetter;
            _DescriptionGetter = descriptionGetter;
            _IsVisibleGetter = isVisibleGetter;
            _IsEnabledGetter = isEnabledGetter;
            _IconGetter = iconGetter;
            _TypeGetter = styleGetter;
            _BadgeCountGetter = badgeCountGetter;
        }

        protected override string ComputeTitle() => _TitleGetter?.Invoke();

        protected override string ComputeDescription() => _DescriptionGetter?.Invoke();

        protected override bool? ComputeIsVisible() => _IsVisibleGetter?.Invoke();

        protected override bool? ComputeIsEnabled() => _IsEnabledGetter?.Invoke();

        protected override string ComputeIcon() => _IconGetter?.Invoke();

        protected override BorderStyle? ComputeStyle() => _TypeGetter?.Invoke();

        protected override int? ComputeBadgeCount() => _BadgeCountGetter?.Invoke();

        public override async void Execute()
        {
            try
            {
                IsExecuting = true;

                await _Execute();
            }
            finally
            {
                IsExecuting = false;
            }
        }

        #endregion Instance Members
    }
}
