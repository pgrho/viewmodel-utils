namespace Shipwreck.ViewModelUtils.Components;

public sealed class SelectionTableColumn : TableColumn
{
    public SelectionTableColumn(Func<object, bool> isReadOnly)
    {
        IsReadOnlyDelegate = isReadOnly;
    }

    public override void RenderHeader(RenderTreeBuilder builder, object dataContext)
    {
        builder.OpenComponent<TableHeaderSelectionCell>(0);
        if (dataContext is ISortablePageViewModel p)
        {
            builder.AddAttribute(1, nameof(TableHeaderSelectionCell.DataContext), p);
        }
        builder.CloseComponent();
    }

    public override void RenderCell(RenderTreeBuilder builder, object dataContext)
    {
        builder.OpenComponent<BooleanCell>(1);
        builder.AddAttribute(2, nameof(BooleanCell.IsChecked), (dataContext as ISelectable)?.IsSelected ?? false);
        builder.AddAttribute(3, nameof(BooleanCell.IsCheckedChanged), (Action<bool>)(v =>
        {
            if (dataContext is ISelectable s)
            {
                s.IsSelected = v;
            }
        }));
        builder.CloseComponent();
    }
}
