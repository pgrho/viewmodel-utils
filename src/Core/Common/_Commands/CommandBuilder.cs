namespace Shipwreck.ViewModelUtils;

public class CommandBuilder : CommandBuilderBase
{
    public Action ExecutionHandler { get; set; }

    public override CommandViewModelBase Build()
    {
        CommandViewModelBase c = null;
        return c = CommandViewModel.Create(
            async () =>
            {
                try
                {
                    ExecutingCallback?.Invoke(c);
                    ExecutionHandler();
                }
                finally
                {
                    ExecutedCallback?.Invoke(c);
                }
            },
            title: Title,
            titleGetter: TitleGetter,
            mnemonic: Mnemonic,
            mnemonicGetter: MnemonicGetter,
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
