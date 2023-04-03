namespace Shipwreck.ViewModelUtils.Demo.Blazor.ViewModels;

public abstract class PageViewModel : FrameworkPageViewModel
{
    protected PageViewModel(PageBase page)
        : base(page)
    {
    }

    public new PageBase Page => (PageBase)base.Page;

    protected override IInteractionService GetInteractionService()
        => Page?.Interaction;
}
