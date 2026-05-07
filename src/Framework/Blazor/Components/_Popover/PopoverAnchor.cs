using Microsoft.AspNetCore.Components.Web;

using KeyboardEventArgs = Microsoft.AspNetCore.Components.Web.KeyboardEventArgs;
using MouseEventArgs = Microsoft.AspNetCore.Components.Web.MouseEventArgs;

namespace Shipwreck.ViewModelUtils.Components;

public abstract class PopoverAnchor<T> : BindableComponentBase<T>
    where T : class
{
    protected ElementReference TargetElement { get; set; }

    public PopoverFrame? Frame { get; protected set; }

    [Parameter]
    public bool IsPrimary { get; set; }

    [Parameter]
    public int? TabIndex { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object> AdditionalAttributes { get; set; }

    [Parameter]
    public ICommand Command { get; set; }

    [Parameter]
    public PopoverTargetCommandMode CommandMode { get; set; } = PopoverTargetCommandMode.Replace;

    protected virtual int? GetTabIndex() => TabIndex;

    protected virtual bool ShouldPopover(KeyboardEventArgs e) => e.Key == " " || e.Key == "Enter";

    protected virtual bool ShouldPopover(MouseEventArgs e) => true;
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

            Frame?.OnButtonFocus();
        }
    }
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
            Frame?.OnButtonFocus();
        }
    }

    protected void OnBlur() => Frame?.OnButtonBlur(TargetElement);
}
