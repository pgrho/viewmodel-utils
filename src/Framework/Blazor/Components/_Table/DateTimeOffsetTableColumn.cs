namespace Shipwreck.ViewModelUtils.Components;

public sealed class DateTimeOffsetTableColumn : TableColumn
{
    public Func<object, DateTimeOffset?> GetValueDelegate { get; set; }

    public override void RenderCell(RenderTreeBuilder builder, object dataContext)
    {
        builder.OpenComponent<DateTimeOffsetCell>(1);
        builder.AddAttribute(2, nameof(DateTimeOffsetCell.Value), GetValueDelegate?.Invoke(dataContext));
        builder.CloseComponent();
    }
}
