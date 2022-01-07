namespace Shipwreck.ViewModelUtils;

public sealed partial class CommandViewModel : CommandViewModelBase
{
    #region Instance Members

    private readonly Action _Execute;
    private readonly Func<string> _TitleGetter = null;
    private readonly Func<string> _MnemonicGetter = null;
    private readonly Func<string> _DescriptionGetter = null;
    private readonly Func<bool> _IsVisibleGetter = null;
    private readonly Func<bool> _IsEnabledGetter = null;
    private readonly Func<string> _IconGetter = null;
    private readonly Func<BorderStyle> _TypeGetter = null;
    private readonly Func<int> _BadgeCountGetter = null;
    private readonly Func<string> _HrefGetter = null;

    private CommandViewModel(
        Action execute
        , string title = null, Func<string> titleGetter = null
        , string mnemonic = null, Func<string> mnemonicGetter = null
        , string description = null, Func<string> descriptionGetter = null
        , bool isVisible = true, Func<bool> isVisibleGetter = null
        , bool isEnabled = true, Func<bool> isEnabledGetter = null
        , string icon = null, Func<string> iconGetter = null
        , BorderStyle style = default, Func<BorderStyle> styleGetter = null
        , int badgeCount = 0, Func<int> badgeCountGetter = null
        , string href = null, Func<string> hrefGetter = null
        )
        : base(
              title: titleGetter?.Invoke() ?? title
              , mnemonic: mnemonicGetter?.Invoke() ?? mnemonic
              , description: descriptionGetter?.Invoke() ?? description
              , isVisible: isVisibleGetter?.Invoke() ?? isVisible
              , isEnabled: isEnabledGetter?.Invoke() ?? isEnabled
              , icon: iconGetter?.Invoke() ?? icon
              , style: styleGetter?.Invoke() ?? style
              , badgeCount: badgeCountGetter?.Invoke() ?? badgeCount
              , href: hrefGetter?.Invoke() ?? href)
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
    }

    protected override string ComputeTitle() => _TitleGetter?.Invoke();
    protected override string ComputeMnemonic() => _MnemonicGetter?.Invoke();

    protected override string ComputeDescription() => _DescriptionGetter?.Invoke();

    protected override bool? ComputeIsVisible() => _IsVisibleGetter?.Invoke();

    protected override bool? ComputeIsEnabled() => _IsEnabledGetter?.Invoke();

    protected override string ComputeIcon() => _IconGetter?.Invoke();

    protected override BorderStyle? ComputeStyle() => _TypeGetter?.Invoke();

    protected override int? ComputeBadgeCount() => _BadgeCountGetter?.Invoke();

    protected override string ComputeHref() => _HrefGetter?.Invoke();

    public override void Execute()
    {
        try
        {
            IsExecuting = true;

            _Execute();
        }
        finally
        {
            IsExecuting = false;
        }
    }

    #endregion Instance Members

    public static CommandViewModelBase Create(
        Action execute,
        string title = null, Func<string> titleGetter = null,
        string mnemonic = null, Func<string> mnemonicGetter = null,
        string description = null, Func<string> descriptionGetter = null,
        bool isVisible = true, Func<bool> isVisibleGetter = null,
        bool isEnabled = true, Func<bool> isEnabledGetter = null,
        string icon = null, Func<string> iconGetter = null,
        BorderStyle style = default, Func<BorderStyle> styleGetter = null,
        int badgeCount = 0, Func<int> badgeCountGetter = null,
        string href = null, Func<string> hrefGetter = null)
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
            hrefGetter: hrefGetter);
    }

    public static CommandViewModelBase CreateAsync(
        Func<Task> execute,
        string title = null, Func<string> titleGetter = null,
        string mnemonic = null, Func<string> mnemonicGetter = null,
        string description = null, Func<string> descriptionGetter = null,
        bool isVisible = true, Func<bool> isVisibleGetter = null,
        bool isEnabled = true, Func<bool> isEnabledGetter = null,
        string icon = null, Func<string> iconGetter = null,
        BorderStyle style = default, Func<BorderStyle> styleGetter = null,
        int badgeCount = 0, Func<int> badgeCountGetter = null,
        string href = null, Func<string> hrefGetter = null)
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
            badgeCountGetter: badgeCountGetter);
    }
}
