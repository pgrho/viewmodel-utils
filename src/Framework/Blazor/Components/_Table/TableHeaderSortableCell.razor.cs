namespace Shipwreck.ViewModelUtils.Components;

public partial class TableHeaderSortableCell : BindableComponentBase<IPaginatable>
{
    [CascadingParameter]
    public IPaginatable SearchPage
    {
        get => DataContext;
        set => DataContext = value;
    }

    [Parameter]
    public string Header { get; set; }

    [Parameter]
    public string Icon { get; set; }

    [Parameter]
    public string Description { get; set; }

    [Parameter]
    public string SortKey
    {
        get => SortKeys?.FirstOrDefault();
        set => SortKeys = string.IsNullOrWhiteSpace(value) ? null : new[] { value };
    }

    [Parameter]
    public IReadOnlyList<string> SortKeys { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object> AdditionalAttributes { get; set; }

    protected override bool OnDataContextPropertyChanged(string propertyName)
    {
        switch (propertyName)
        {
            case nameof(DataContext.Order):
                return true;
        }
        return false;
    }

    [CascadingParameter]
    public IHasPopoverPresenter HasPopoverPresenter { get; set; }

    [Parameter]
    public string FilterKey { get; set; }

    private string GetFilterKey() => FilterKey ?? SortKeys.FirstOrDefault();

    private ElementReference _FilterButton;

    internal string Filter
    {
        get => DataContext is IFilterable f
            && GetFilterKey() is string s
            && f.IsFilterSupported(s) ? f.GetFilter(s) : null;
        set
        {
            if (DataContext is IFilterable f
                && GetFilterKey() is string s
                && f.IsFilterSupported(s))
            {
                f.SetFilter(s, value);
            }
        }
    }
}
