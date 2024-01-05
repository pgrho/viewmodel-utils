using Microsoft.AspNetCore.Components;
using Shipwreck.ViewModelUtils.Components;
using Shipwreck.ViewModelUtils.Demo.Blazor.Shared;

namespace Shipwreck.ViewModelUtils.Demo.Blazor.Pages;

public abstract class PageBase : FrameworkPageBase, IHasPopoverPresenter, IHasModalPresenter
{
    [Inject]
    public InteractionService Interaction { get; set; }

    [CascadingParameter]
    public MainLayout MainLayout { get; set; }

    public ModalPresenterBase ModalPresenter => MainLayout?.ModalPresenter;
    public ModalPresenterBase PopoverPresenter => MainLayout?.PopoverPresenter;
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
