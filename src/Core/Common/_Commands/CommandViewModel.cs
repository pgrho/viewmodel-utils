namespace Shipwreck.ViewModelUtils;

public sealed partial class CommandViewModel : CommandViewModelBase
{
    #region Instance Members

    private readonly Action<CommandViewModelBase> _Execute;
    private readonly Func<CommandViewModelBase, string?>? _TitleGetter = null;
    private readonly Func<CommandViewModelBase, string?>? _MnemonicGetter = null;
    private readonly Func<CommandViewModelBase, string?>? _DescriptionGetter = null;
    private readonly Func<CommandViewModelBase, bool>? _IsVisibleGetter = null;
    private readonly Func<CommandViewModelBase, bool>? _IsEnabledGetter = null;
    private readonly Func<CommandViewModelBase, string?>? _IconGetter = null;
    private readonly Func<CommandViewModelBase, BorderStyle>? _TypeGetter = null;
    private readonly Func<CommandViewModelBase, int>? _BadgeCountGetter = null;
    private readonly Func<CommandViewModelBase, string?>? _HrefGetter = null;

    private readonly ICommandViewModelHandler? _Handler;

    private CommandViewModel(
        Action<CommandViewModelBase> execute
        , string? title = null, Func<CommandViewModelBase, string?>? titleGetter = null
        , string? mnemonic = null, Func<CommandViewModelBase, string?>? mnemonicGetter = null
        , string? description = null, Func<CommandViewModelBase, string?>? descriptionGetter = null
        , bool isVisible = true, Func<CommandViewModelBase, bool>? isVisibleGetter = null
        , bool isEnabled = true, Func<CommandViewModelBase, bool>? isEnabledGetter = null
        , string? icon = null, Func<CommandViewModelBase, string?>? iconGetter = null
        , BorderStyle style = default, Func<CommandViewModelBase, BorderStyle>? styleGetter = null
        , int badgeCount = 0, Func<CommandViewModelBase, int>? badgeCountGetter = null
        , string? href = null, Func<CommandViewModelBase, string?>? hrefGetter = null
        , ICommandViewModelHandler? handler = null
        )
        : base(
              title: title
              , mnemonic: mnemonic
              , description: description
              , isVisible: isVisible
              , isEnabled: isEnabled
              , icon: icon
              , style: style
              , badgeCount: badgeCount
              , href: href)
    {
        _Execute = execute;
        _TitleGetter = titleGetter;
        _MnemonicGetter = mnemonicGetter;
        _DescriptionGetter = descriptionGetter;
        _IsVisibleGetter = isVisibleGetter;
        _IsEnabledGetter = isEnabledGetter;
        _HrefGetter = hrefGetter;
        _IconGetter = iconGetter;
        _TypeGetter = styleGetter;
        _BadgeCountGetter = badgeCountGetter;

        _Handler = handler;

        Invalidate();
    }

    private CommandViewModel(
        Action execute
        , string? title = null, Func<string?>? titleGetter = null
        , string? mnemonic = null, Func<string?>? mnemonicGetter = null
        , string? description = null, Func<string?>? descriptionGetter = null
        , bool isVisible = true, Func<bool>? isVisibleGetter = null
        , bool isEnabled = true, Func<bool>? isEnabledGetter = null
        , string? icon = null, Func<string?>? iconGetter = null
        , BorderStyle style = default, Func<BorderStyle>? styleGetter = null
        , int badgeCount = 0, Func<int>? badgeCountGetter = null
        , string? href = null, Func<string?>? hrefGetter = null
        , ICommandViewModelHandler? handler = null
        )
        : base(
              title: title
              , mnemonic: mnemonic
              , description: description
              , isVisible: isVisible
              , isEnabled: isEnabled
              , icon: icon
              , style: style
              , badgeCount: badgeCount
              , href: href)
    {
        _Execute = _ => execute();


        _TitleGetter = ExpressionHelper.AddCommandArgument(titleGetter);
        _MnemonicGetter = ExpressionHelper.AddCommandArgument(mnemonicGetter);
        _DescriptionGetter = ExpressionHelper.AddCommandArgument(descriptionGetter);
        _IsVisibleGetter = ExpressionHelper.AddCommandArgument(isVisibleGetter);
        _IsEnabledGetter = ExpressionHelper.AddCommandArgument(isEnabledGetter);
        _HrefGetter = ExpressionHelper.AddCommandArgument(hrefGetter);
        _IconGetter = ExpressionHelper.AddCommandArgument(iconGetter);
        _TypeGetter = ExpressionHelper.AddCommandArgument(styleGetter);
        _BadgeCountGetter = ExpressionHelper.AddCommandArgument(badgeCountGetter);

        _Handler = handler;

        Invalidate();
    }

    protected override string? ComputeTitle() => _TitleGetter?.Invoke(this);

    protected override string? ComputeMnemonic() => _MnemonicGetter?.Invoke(this);

    protected override string? ComputeDescription() => _DescriptionGetter?.Invoke(this);

    protected override bool? ComputeIsVisible() => _IsVisibleGetter?.Invoke(this);

    protected override bool? ComputeIsEnabled() => _IsEnabledGetter?.Invoke(this);

    protected override string? ComputeIcon() => _IconGetter?.Invoke(this);

    protected override BorderStyle? ComputeStyle() => _TypeGetter?.Invoke(this);

    protected override int? ComputeBadgeCount() => _BadgeCountGetter?.Invoke(this);

    protected override string? ComputeHref() => _HrefGetter?.Invoke(this);

    public override void Execute()
    {
        try
        {
            _Handler?.OnCommandExecuting(this);
            IsExecuting = true;

            _Execute(this);
        }
        finally
        {
            IsExecuting = false;

            _Handler?.OnCommandExecuted(this);
        }
    }

    #endregion Instance Members

    public static CommandViewModelBase Create(
        Action execute,
        string? title = null, Func<string?>? titleGetter = null,
        string? mnemonic = null, Func<string?>? mnemonicGetter = null,
        string? description = null, Func<string?>? descriptionGetter = null,
        bool isVisible = true, Func<bool>? isVisibleGetter = null,
        bool isEnabled = true, Func<bool>? isEnabledGetter = null,
        string? icon = null, Func<string?>? iconGetter = null,
        BorderStyle style = default, Func<BorderStyle>? styleGetter = null,
        int badgeCount = 0, Func<int>? badgeCountGetter = null,
        string? href = null, Func<string?>? hrefGetter = null,
        ICommandViewModelHandler? handler = null)
    {
        return new CommandViewModel(
            execute,
            title: title,
            titleGetter: titleGetter,
            mnemonic: mnemonic,
            mnemonicGetter: mnemonicGetter,
            description: description,
            descriptionGetter: descriptionGetter,
            isVisible: isVisible,
            isVisibleGetter: isVisibleGetter,
            isEnabled: isEnabled,
            isEnabledGetter: isEnabledGetter,
            icon: icon,
            iconGetter: iconGetter,
            style: style,
            styleGetter: styleGetter,
            badgeCount: badgeCount,
            badgeCountGetter: badgeCountGetter,
            href: href,
            hrefGetter: hrefGetter,
            handler: handler);
    }
    public static CommandViewModelBase Create(
        Action<CommandViewModelBase> execute,
        string? title = null, Func<CommandViewModelBase, string?>? titleGetter = null,
        string? mnemonic = null, Func<CommandViewModelBase, string?>? mnemonicGetter = null,
        string? description = null, Func<CommandViewModelBase, string?>? descriptionGetter = null,
        bool isVisible = true, Func<CommandViewModelBase, bool>? isVisibleGetter = null,
        bool isEnabled = true, Func<CommandViewModelBase, bool>? isEnabledGetter = null,
        string? icon = null, Func<CommandViewModelBase, string?>? iconGetter = null,
        BorderStyle style = default, Func<CommandViewModelBase, BorderStyle>? styleGetter = null,
        int badgeCount = 0, Func<CommandViewModelBase, int>? badgeCountGetter = null,
        string? href = null, Func<CommandViewModelBase, string?>? hrefGetter = null,
        ICommandViewModelHandler? handler = null)
    {
        return new CommandViewModel(
            execute,
            title: title,
            titleGetter: titleGetter,
            mnemonic: mnemonic,
            mnemonicGetter: mnemonicGetter,
            description: description,
            descriptionGetter: descriptionGetter,
            isVisible: isVisible,
            isVisibleGetter: isVisibleGetter,
            isEnabled: isEnabled,
            isEnabledGetter: isEnabledGetter,
            icon: icon,
            iconGetter: iconGetter,
            style: style,
            styleGetter: styleGetter,
            badgeCount: badgeCount,
            badgeCountGetter: badgeCountGetter,
            href: href,
            hrefGetter: hrefGetter,
            handler: handler);
    }

    public static CommandViewModelBase CreateAsync(
        Func<Task> execute,
        string? title = null, Func<string?>? titleGetter = null,
        string? mnemonic = null, Func<string?>? mnemonicGetter = null,
        string? description = null, Func<string?>? descriptionGetter = null,
        bool isVisible = true, Func<bool>? isVisibleGetter = null,
        bool isEnabled = true, Func<bool>? isEnabledGetter = null,
        string? icon = null, Func<string>? iconGetter = null,
        BorderStyle style = default, Func<BorderStyle>? styleGetter = null,
        int badgeCount = 0, Func<int>? badgeCountGetter = null,
        string? href = null, Func<string?>? hrefGetter = null,
        ICommandViewModelHandler? handler = null)
    {
        return new AsyncCommandViewModel(
            execute,
            title: title,
            titleGetter: titleGetter,
            mnemonic: mnemonic,
            mnemonicGetter: mnemonicGetter,
            description: description,
            descriptionGetter: descriptionGetter,
            isVisible: isVisible,
            isVisibleGetter: isVisibleGetter,
            isEnabled: isEnabled,
            isEnabledGetter: isEnabledGetter,
            icon: icon,
            iconGetter: iconGetter,
            style: style,
            styleGetter: styleGetter,
            badgeCount: badgeCount,
            badgeCountGetter: badgeCountGetter,
            href: href,
            hrefGetter: hrefGetter,
            handler: handler);
    }

    public static CommandViewModelBase CreateAsync(
        Func<CommandViewModelBase, Task> execute,
        string? title = null, Func<CommandViewModelBase, string?>? titleGetter = null,
        string? mnemonic = null, Func<CommandViewModelBase, string?>? mnemonicGetter = null,
        string? description = null, Func<CommandViewModelBase, string?>? descriptionGetter = null,
        bool isVisible = true, Func<CommandViewModelBase, bool>? isVisibleGetter = null,
        bool isEnabled = true, Func<CommandViewModelBase, bool>? isEnabledGetter = null,
        string? icon = null, Func<CommandViewModelBase, string?>? iconGetter = null,
        BorderStyle style = default, Func<CommandViewModelBase, BorderStyle>? styleGetter = null,
        int badgeCount = 0, Func<CommandViewModelBase, int>? badgeCountGetter = null,
        string? href = null, Func<CommandViewModelBase, string?>? hrefGetter = null,
        ICommandViewModelHandler? handler = null)
    {
        return new AsyncCommandViewModel(
            execute,
            title: title,
            titleGetter: titleGetter,
            mnemonic: mnemonic,
            mnemonicGetter: mnemonicGetter,
            description: description,
            descriptionGetter: descriptionGetter,
            isVisible: isVisible,
            isVisibleGetter: isVisibleGetter,
            isEnabled: isEnabled,
            isEnabledGetter: isEnabledGetter,
            icon: icon,
            iconGetter: iconGetter,
            style: style,
            styleGetter: styleGetter,
            badgeCount: badgeCount,
            badgeCountGetter: badgeCountGetter,
            href: href,
            hrefGetter: hrefGetter,
            handler: handler);
    }
}
