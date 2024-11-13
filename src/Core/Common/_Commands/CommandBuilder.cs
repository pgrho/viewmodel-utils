namespace Shipwreck.ViewModelUtils;

public class CommandBuilder : CommandBuilderBase
{
    [Obsolete]
    public CommandBuilder() { }

    public CommandBuilder(Action executionHandler)
    {
        ExecutionHandler = _ => executionHandler();
    }

    public CommandBuilder(Action<CommandViewModelBase> executionHandler)
    {
        ExecutionHandler = executionHandler;
    }

    public Action<CommandViewModelBase> ExecutionHandler { get; set; }

    public override CommandViewModelBase Build()
    {
        CommandViewModelBase? c = null;
        return c = CommandViewModel.Create(
            ExecutionHandler,
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
            badgeCountGetter: BadgeCountGetter,
            handler: GetHandler());
    }
}
