namespace Shipwreck.ViewModelUtils;

public abstract class PageCommandBuilder : CommandBuilderBase
{
    protected PageCommandBuilder(FrameworkPageViewModel page)
    {
        Page = page;
    }

    protected FrameworkPageViewModel Page { get; }

    protected virtual void OnExecuted()
    {
        var ne = GetNavigationEntry();
        if (ne != null)
        {
            Page?.NavigateTo(ne);
        }
    }

    protected abstract NavigationEntry GetNavigationEntry();

    protected string ComputeHref(CommandViewModelBase command)
        => HrefGetter?.Invoke(command) ?? Href ?? GetNavigationEntry()?.Path;

    protected virtual string ComputeIcon(CommandViewModelBase command)
        => IconGetter?.Invoke(command) ?? Icon;

    protected virtual string ComputeTitle(CommandViewModelBase command)
        => TitleGetter?.Invoke(command) ?? Title;

    protected virtual string ComputeDescription(CommandViewModelBase command)
        => DescriptionGetter?.Invoke(command) ?? Description;

    protected virtual bool ComputeIsVisible(CommandViewModelBase command)
        => IsVisibleGetter?.Invoke(command) ?? IsVisible ?? IsSupported();

    protected virtual bool ComputeIsEnabled(CommandViewModelBase command)
        => IsSupported() && (IsEnabledGetter?.Invoke(command) ?? IsEnabled ?? true);

    protected bool IsSupported()
        => GetNavigationEntry() is var e
        && e?.Path != null
        && Page?.Navigation is var n
        && n.IsSupported(Page, e);

    public override CommandViewModelBase Build()
        => CommandViewModel.Create(
            _ => OnExecuted(),
            titleGetter: ComputeTitle,
            descriptionGetter: ComputeDescription,
            isVisibleGetter: ComputeIsVisible,
            isEnabledGetter: ComputeIsEnabled,
            iconGetter: ComputeIcon,
            style: Style ?? default,
            styleGetter: StyleGetter,
            hrefGetter: ComputeHref,
            badgeCount: BadgeCount ?? 0,
            badgeCountGetter: BadgeCountGetter);
}

public abstract class FrameworkPageCommandBuilder : CommandBuilderBase
{
    protected FrameworkPageCommandBuilder(FrameworkPageViewModel page)
    {
        Page = page;
    }

    [Obsolete("", true)]
    public new string Href
    {
        get => base.Href;
        set => base.Href = value;
    }

    [Obsolete("", true)]
    public new Func<CommandViewModelBase, string> HrefGetter
    {
        get => base.HrefGetter;
        set => base.HrefGetter = value;
    }

    protected FrameworkPageViewModel Page { get; }

    protected abstract NavigationEntry CreateEntry();

    protected virtual void OnExecuted()
    {
        var e = CreateEntry();
        if (e != null && Page.IsSupported(e))
        {
            Page.NavigateTo(e);
        }
    }

    protected virtual string ComputeHref()
        => CreateEntry()?.Path is string p ? p.StartsWith("/") ? p.Substring(1) : p : null;

    protected virtual string ComputeIcon() => null;

    protected virtual string ComputeDescription() => null;

    protected virtual bool ComputeIsVisible()
        => Page.IsSupported(CreateEntry());

    public sealed override CommandViewModelBase Build()
        => CommandViewModel.Create(
            _ => OnExecuted()
            , title: Title
            , titleGetter: TitleGetter
            , description: Description ?? ComputeDescription()
            , descriptionGetter: DescriptionGetter
            , isVisibleGetter: IsVisibleGetter != null
                ? c => ComputeIsVisible() && IsVisibleGetter(c)
                : (_ => ComputeIsVisible())
            , isEnabled: IsEnabled ?? true
            , isEnabledGetter: IsEnabledGetter
            , icon: Icon ?? ComputeIcon()
            , iconGetter: IconGetter
            , style: Style ?? default
            , styleGetter: StyleGetter
            , hrefGetter: _ => ComputeHref()
            , badgeCount: BadgeCount ?? 0
            , badgeCountGetter: BadgeCountGetter);
}
