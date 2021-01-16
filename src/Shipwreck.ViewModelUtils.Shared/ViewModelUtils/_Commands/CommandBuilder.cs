using System;

namespace Shipwreck.ViewModelUtils
{
    public class CommandBuilder : CommandBuilderBase
    {
        public Action ExecutionHandler { get; set; }

        public override CommandViewModelBase Build()
            => CommandViewModel.Create(
                ExecutionHandler,
                title: Title,
                titleGetter: TitleGetter,
                description: Description,
                descriptionGetter: DescriptionGetter,
                isVisible: IsVisible ?? true,
                isVisibleGetter: IsVisibleGetter,
                isEnabled: IsEnabled ?? true,
                isEnabledGetter: IsEnabledGetter,
                icon: Icon,
                iconGetter: IconGetter,
                style: Style ?? default,
                styleGetter: StyleGetter,
                href: Href,
                hrefGetter: HrefGetter,
                badgeCount: BadgeCount ?? 0,
                badgeCountGetter: BadgeCountGetter);
    }
}
