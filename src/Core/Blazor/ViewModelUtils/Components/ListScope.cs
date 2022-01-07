namespace Shipwreck.ViewModelUtils.Components;

public partial class ListScope<T> : ListComponentBase<T>
    where T : class
{
    [Parameter]
    public RenderFragment ChildContent { get; set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
        => builder.AddContent(0, ChildContent);
}
