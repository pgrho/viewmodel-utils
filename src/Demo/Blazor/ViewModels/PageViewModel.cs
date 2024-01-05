using Shipwreck.ViewModelUtils.Components;

namespace Shipwreck.ViewModelUtils.Demo.Blazor.ViewModels;

public abstract class PageViewModel : FrameworkPageViewModel, IHasPopoverPresenter, IHasModalPresenter
{
    protected PageViewModel(PageBase page)
        : base(page)
    {
    }

    public new PageBase Page => (PageBase)base.Page;

    protected override IInteractionService GetInteractionService()
        => Page?.Interaction;

    public ModalPresenterBase ModalPresenter => Page?.ModalPresenter;
    public ModalPresenterBase PopoverPresenter => Page?.PopoverPresenter;
}
