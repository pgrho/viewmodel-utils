namespace Shipwreck.ViewModelUtils.Components;

public class DataContextScope : BindableComponentBase<object>
{
    protected override void BuildRenderTree(RenderTreeBuilder builder)
        => builder.AddContent(0, ChildContent);

    #region ChildContent

    private RenderFragment _ChildContent;

    [Parameter]
    public RenderFragment ChildContent
    {
        get => _ChildContent;
        set => SetProperty(ref _ChildContent, value);
    }

    #endregion ChildContent

    #region DependsOnProperties

    private IEnumerable<string> _DependsOnProperties;

    [Parameter]
    public IEnumerable<string> DependsOnProperties
    {
        get => _DependsOnProperties;
        set => SetProperty(ref _DependsOnProperties, value);
    }

    #endregion DependsOnProperties

    #region IgnoresProperties

    private IEnumerable<string> _IgnoresProperties;

    [Parameter]
    public IEnumerable<string> IgnoresProperties
    {
        get => _IgnoresProperties;
        set => SetProperty(ref _IgnoresProperties, value);
    }

    #endregion IgnoresProperties

    [Parameter]
    public Func<string, bool> RequestedFocus { get; set; }

    protected override bool OnDataContextPropertyChanged(string propertyName)
        => DependsOnProperties?.Contains(propertyName) != false
        && IgnoresProperties?.Contains(propertyName) != true;

    protected override bool OnDataContextRequestedFocus(string propertyName)
        => RequestedFocus?.Invoke(propertyName) ?? base.OnDataContextRequestedFocus(propertyName);
}
