using Shipwreck.ViewModelUtils.Components;

namespace Shipwreck.ViewModelUtils.Demo.Blazor.ViewModels;

public abstract class PageViewModel : FrameworkPageViewModel, IHasToastPresenter
{
    protected PageViewModel(PageBase page)
        : base(page)
    {
    }

    public new PageBase Page => (PageBase)base.Page;

    ToastPresenterBase IHasToastPresenter.ToastPresenter => (Page as IHasToastPresenter)?.ToastPresenter;

    protected override IInteractionService GetInteractionService()
        => Page?.Interaction;
}
