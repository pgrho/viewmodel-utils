using Microsoft.AspNetCore.Components.Web;

namespace Shipwreck.ViewModelUtils.Components;

public abstract class PopoverTarget<T> : BindableComponentBase<T>
    where T : class
{
    protected ElementReference TargetElement { get; set; }

    [CascadingParameter]
    public IHasPopoverPresenter PopoverPresenterProvider { get; set; }

    [CascadingParameter]
    public IContainerElementProvider ContainerElementProvider { get; set; }

    [Parameter]
    public ICommand Command { get; set; }

    [Parameter]
    public PopoverTargetCommandMode CommandMode { get; set; } = PopoverTargetCommandMode.Replace;

    protected ElementReference ContainerElement => ContainerElementProvider?.Container ?? default;

    protected void OnKeyDown(KeyboardEventArgs e)
    {
        if (ShouldPopover(e))
        {
            if (Command?.CanExecute(DataContext) == true)
            {
                Command.Execute(DataContext);
                if (CommandMode == PopoverTargetCommandMode.Replace)
                {
                    return;
                }
            }
            ShowPopover(DataContext);
        }
    }

    protected virtual bool ShouldPopover(KeyboardEventArgs e) => e.Key == " " || e.Key == "Enter";

    protected virtual bool ShouldPopover(MouseEventArgs e) => true;

    protected void OnClick(MouseEventArgs e)
    {
        if (ShouldPopover(e))
        {
            if (Command?.CanExecute(DataContext) == true)
            {
                Command.Execute(DataContext);
                if (CommandMode == PopoverTargetCommandMode.Replace)
                {
                    return;
                }
            }
            ShowPopover(DataContext);
        }
    }

    protected abstract void ShowPopover(T dataContext);
}
