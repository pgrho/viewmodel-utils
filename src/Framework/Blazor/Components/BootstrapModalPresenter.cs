namespace Shipwreck.ViewModelUtils.Components;

public sealed class BootstrapModalPresenter : ModalPresenterBase
{
    [Inject]
    public IJSRuntime JS { get; set; }

    protected override void BuildModalBackdropTree(RenderTreeBuilder builder, int sequence)
    {
        builder.AddMarkupContent(sequence, "<div class=\"modal-backdrop show\"></div>");
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await (base.OnAfterRenderAsync(firstRender) ?? Task.CompletedTask);

        if (ModalType == null && !firstRender)
        {
            JS.InvokeVoidAsync("Shipwreck.ViewModelUtils.toggleModal", null, false, null);
        }
    }
}
