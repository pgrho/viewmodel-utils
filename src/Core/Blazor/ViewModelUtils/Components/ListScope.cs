namespace Shipwreck.ViewModelUtils.Components;

public partial class ListScope<T> : ListComponentBase<T>
    where T : class
{
    #region ChildContent

    private RenderFragment _ChildContent;

    [Parameter]
    public RenderFragment ChildContent
    {
        get => _ChildContent;
        set => SetProperty(ref _ChildContent, value);
    }

    #endregion ChildContent

    protected override void BuildRenderTree(RenderTreeBuilder builder)
        => builder.AddContent(0, ChildContent);
}
