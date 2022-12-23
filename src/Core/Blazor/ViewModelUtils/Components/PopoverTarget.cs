using Microsoft.AspNetCore.Components.Web;
using KeyboardEventArgs = Microsoft.AspNetCore.Components.Web.KeyboardEventArgs;
using MouseEventArgs = Microsoft.AspNetCore.Components.Web.MouseEventArgs;

namespace Shipwreck.ViewModelUtils.Components;

public abstract class PopoverTarget<T> : BindableComponentBase<T>
    where T : class
{
    protected ElementReference TargetElement { get; set; }

    #region PopoverPresenterProvider

    private IHasPopoverPresenter _PopoverPresenterProvider;

    [CascadingParameter]
    public IHasPopoverPresenter PopoverPresenterProvider
    {
        get => _PopoverPresenterProvider;
        set => SetProperty(ref _PopoverPresenterProvider, value);
    }

    #endregion PopoverPresenterProvider

    #region ContainerElementProvider

    private IContainerElementProvider _ContainerElementProvider;

    [CascadingParameter]
    public IContainerElementProvider ContainerElementProvider
    {
        get => _ContainerElementProvider;
        set => SetProperty(ref _ContainerElementProvider, value);
    }

    #endregion ContainerElementProvider

    #region Command

    private ICommand _Command;

    [Parameter]
    public ICommand Command
    {
        get => _Command;
        set => SetProperty(ref _Command, value);
    }

    #endregion Command

    #region CommandMode

    private PopoverTargetCommandMode _CommandMode = PopoverTargetCommandMode.Replace;

    [Parameter]
    public PopoverTargetCommandMode CommandMode
    {
        get => _CommandMode;
        set => SetProperty(ref _CommandMode, value);
    }

    #endregion CommandMode

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
