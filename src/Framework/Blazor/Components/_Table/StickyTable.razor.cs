namespace Shipwreck.ViewModelUtils.Components;

public partial class StickyTable<T> : ListComponentBase<T>
    where T : class
{
    private ElementReference _Element;

    public ElementReference Container => _Element;

    #region HeaderTemplate

    private RenderFragment _HeaderTemplate;

    [Parameter]
    public RenderFragment HeaderTemplate
    {
        get => _HeaderTemplate;
        set => SetProperty(ref _HeaderTemplate, value);
    }

    #endregion HeaderTemplate

    #region FooterTemplate

    private RenderFragment _FooterTemplate;

    [Parameter]
    public RenderFragment FooterTemplate
    {
        get => _FooterTemplate;
        set => SetProperty(ref _FooterTemplate, value);
    }

    #endregion FooterTemplate

    #region ItemTemplate

    private RenderFragment<ItemTemplateContext<T>> _ItemTemplate;

    [Parameter]
    public RenderFragment<ItemTemplateContext<T>> ItemTemplate
    {
        get => _ItemTemplate;
        set => SetProperty(ref _ItemTemplate, value);
    }

    #endregion ItemTemplate

    #region SearchPage

    private ISortablePageViewModel _SearchPage;

    [Parameter]
    public ISortablePageViewModel SearchPage
    {
        get => _SearchPage;
        set => SetProperty(ref _SearchPage, value);
    }

    #endregion SearchPage

    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object> AdditionalAttributes
    {
        get => _AdditionalAttributes;
        set => SetProperty(ref _AdditionalAttributes, value);
    }
    private IDictionary<string, object> _AdditionalAttributes;

    #region TableClass

    private string _TableClass = "table table-sm table-hover";

    [Parameter]
    public string TableClass
    {
        get => _TableClass;
        set => SetProperty(ref _TableClass, value);
    }

    #endregion TableClass

    #region TheadClass

    private string _TheadClass = "thead-dark";

    [Parameter]
    public string TheadClass
    {
        get => _TheadClass;
        set => SetProperty(ref _TheadClass, value);
    }

    #endregion TheadClass
}
