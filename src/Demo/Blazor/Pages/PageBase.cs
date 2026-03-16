using Microsoft.AspNetCore.Components;
using Shipwreck.ViewModelUtils.Components;
using Shipwreck.ViewModelUtils.Demo.Blazor.Shared;

namespace Shipwreck.ViewModelUtils.Demo.Blazor.Pages;

public abstract class PageBase : FrameworkPageBase, IHasToastPresenter, IHasModalPresenter, IHasPopoverPresenter
{
    [CascadingParameter]
    public MainLayout Layout { get; set; }

    [Inject]
    public InteractionService Interaction { get; set; }

    public ToastPresenterBase ToastPresenter => Layout?.ToastPresenter;

    public ModalPresenterBase ModalPresenter => Layout?.ModalPresenter;

    public ModalPresenterBase PopoverPresenter => Layout?.PopoverPresenter;
}

public abstract class PageBase<T> : PageBase
    where T : PageViewModel
{
    public new T DataContext
    {
        get => (T)base.DataContext;
        set => base.DataContext = value;
    }
}
