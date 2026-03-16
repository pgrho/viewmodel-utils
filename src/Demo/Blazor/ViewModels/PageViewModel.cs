using Shipwreck.ViewModelUtils.Components;

namespace Shipwreck.ViewModelUtils.Demo.Blazor.ViewModels;

public abstract class PageViewModel : FrameworkPageViewModel, IHasToastPresenter, IHasModalPresenter, IHasPopoverPresenter
{
    protected PageViewModel(PageBase page)
        : base(page)
    {
    }

    public new PageBase Page => (PageBase)base.Page;

    ToastPresenterBase IHasToastPresenter.ToastPresenter => (Page as IHasToastPresenter)?.ToastPresenter;

    ModalPresenterBase IHasModalPresenter.ModalPresenter => (Page as IHasModalPresenter)?.ModalPresenter;
    ModalPresenterBase IHasPopoverPresenter.PopoverPresenter => (Page as IHasPopoverPresenter)?.PopoverPresenter;

    protected override IInteractionService GetInteractionService()
        => Page?.Interaction;
}
