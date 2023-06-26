namespace Shipwreck.ViewModelUtils.Components;

public abstract class TableColumn
{
    public string Header { get; set; }
    public string SortKey { get; set; }

    public string Description { get; set; }
    public string Icon { get; set; }

    public Func<object, bool> IsReadOnlyDelegate { get; set; }
    public Func<object, bool> IsChangedDelegate { get; set; }

    public Dictionary<string, object> AdditionalAttributes { get; set; }

    public virtual void RenderHeader(RenderTreeBuilder builder, object dataContext)
    {
        builder.OpenComponent<TableHeaderSortableCell>(0);
        if (!string.IsNullOrEmpty(Header))
        {
            builder.AddAttribute(1, nameof(TableHeaderSortableCell.Header), Header);
        }
        if (!string.IsNullOrEmpty(SortKey))
        {
            builder.AddAttribute(2, nameof(TableHeaderSortableCell.SortKey), SortKey);
        }
        if (!string.IsNullOrEmpty(Description))
        {
            builder.AddAttribute(3, nameof(TableHeaderSortableCell.Description), Description);
        }
        if (!string.IsNullOrEmpty(Icon))
        {
            builder.AddAttribute(4, nameof(TableHeaderSortableCell.Icon), Icon);
        }
        if (AdditionalAttributes?.Count > 0)
        {
            builder.AddAttribute(5, nameof(TableHeaderSortableCell.AdditionalAttributes), AdditionalAttributes);
        }
        builder.CloseComponent();
    }

    public abstract void RenderCell(RenderTreeBuilder builder, object dataContext);
}
