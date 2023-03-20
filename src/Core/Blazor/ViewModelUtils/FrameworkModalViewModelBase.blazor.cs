namespace Shipwreck.ViewModelUtils;

public partial class FrameworkModalViewModelBase : IHasJSRuntime
{
    ModalPresenterBase IHasModalPresenter.ModalPresenter => (Page as IHasModalPresenter)?.ModalPresenter;
    ModalPresenterBase IHasPopoverPresenter.PopoverPresenter => (Page as IHasPopoverPresenter)?.PopoverPresenter;

    IBindableComponent IHasBindableComponent.Component => Page?.Page;
}
