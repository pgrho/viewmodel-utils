namespace Shipwreck.ViewModelUtils;

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
    public new Func<string> HrefGetter
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
            OnExecuted,
            title: Title,
            titleGetter: TitleGetter,
            description: Description ?? ComputeDescription(),
            descriptionGetter: DescriptionGetter,
            isVisibleGetter: IsVisibleGetter != null ? () => ComputeIsVisible() && IsVisibleGetter() : (Func<bool>)ComputeIsVisible,
            isEnabled: IsEnabled ?? true,
            isEnabledGetter: IsEnabledGetter,
            icon: Icon ?? ComputeIcon(),
            iconGetter: IconGetter,
            style: Style ?? default,
            styleGetter: StyleGetter,
            hrefGetter: ComputeHref,
            badgeCount: BadgeCount ?? 0,
            badgeCountGetter: BadgeCountGetter);
}
