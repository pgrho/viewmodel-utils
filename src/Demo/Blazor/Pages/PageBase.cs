using Microsoft.AspNetCore.Components;
using Shipwreck.ViewModelUtils.Components;

namespace Shipwreck.ViewModelUtils.Demo.Blazor.Pages;

public abstract class PageBase : FrameworkPageBase
{
    [Inject]
    public InteractionService Interaction { get; set; }
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
