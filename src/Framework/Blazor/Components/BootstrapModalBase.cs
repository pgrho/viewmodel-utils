namespace Shipwreck.ViewModelUtils.Components;

public abstract class BootstrapModalBase<T> : ModalBase<T>
      where T : class
{
    protected override ValueTask ShowAsyncCore()
        => JS.InvokeVoidAsync("Shipwreck.ViewModelUtils.toggleModal", ModalElement, true, DotNetObjectReference.Create(this));

    protected override ValueTask HideAsyncCore()
        => JS.InvokeVoidAsync("Shipwreck.ViewModelUtils.toggleModal", ModalElement, false, DotNetObjectReference.Create(this));

    protected override void OnAfterRender(bool firstRender)
    {
        base.OnAfterRender(firstRender);

        if (firstRender && DataContext is FrameworkModalViewModelBase d)
        {
            d.IsVisible = true;
        }
    }

    [JSInvokable]
    public override void OnClosed()
    {
        base.OnClosed();

        if (DataContext is FrameworkModalViewModelBase d)
        {
            d.IsVisible = false;
        }
    }
}
