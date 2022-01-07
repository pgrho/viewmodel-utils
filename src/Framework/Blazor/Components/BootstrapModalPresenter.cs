namespace Shipwreck.ViewModelUtils.Components;

public sealed class BootstrapModalPresenter : ModalPresenterBase
{
    protected override void BuildModalBackdropTree(RenderTreeBuilder builder, int sequence)
    {
        builder.AddMarkupContent(sequence, "<div class=\"modal-backdrop show\"></div>");
    }
}
