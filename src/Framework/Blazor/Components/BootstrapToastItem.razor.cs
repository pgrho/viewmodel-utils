namespace Shipwreck.ViewModelUtils.Components;

public partial class BootstrapToastItem : BindableComponentBase<ToastData>
{
    private ElementReference _Element;

    [Inject]
    public IJSRuntime JS { get; set; }

    [CascadingParameter]
    public BootstrapToastPresenter ToastPresenter { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await (base.OnAfterRenderAsync(firstRender) ?? Task.CompletedTask);

        if (firstRender)
        {
            await JS.InvokeVoidAsync("Shipwreck.ViewModelUtils.initializeToast", _Element, DotNetObjectReference.Create(this));
        }
    }

    [JSInvokable]
    public void OnHidden() => ToastPresenter?.Source?.Remove(DataContext);
}
