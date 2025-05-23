﻿namespace Shipwreck.ViewModelUtils;

public class AsyncCommandBuilder : CommandBuilderBase
{
    private readonly Func<CommandViewModelBase, Task> _ExecutionHandler;

    public AsyncCommandBuilder(Func<Task> executionHandler)
    {
        _ExecutionHandler = executionHandler.AddCommandArgument()!;
    }

    public AsyncCommandBuilder(Func<CommandViewModelBase, Task> executionHandler)
    {
        _ExecutionHandler = executionHandler;
    }

    public override CommandViewModelBase Build()
    {
        CommandViewModelBase? c = null;
        return c = CommandViewModel.CreateAsync(
            _ExecutionHandler,
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
