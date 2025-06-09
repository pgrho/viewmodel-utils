namespace Shipwreck.ViewModelUtils;

public partial class FrameworkModalViewModelBase : IHasJSRuntime
{
    ModalPresenterBase IHasModalPresenter.ModalPresenter => (Page as IHasModalPresenter)?.ModalPresenter;
    ModalPresenterBase IHasPopoverPresenter.PopoverPresenter => (Page as IHasPopoverPresenter)?.PopoverPresenter;
    ToastPresenterBase IHasToastPresenter.ToastPresenter => (Page as IHasToastPresenter)?.ToastPresenter;

    IBindableComponent IHasBindableComponent.Component => Page?.Page;
}
