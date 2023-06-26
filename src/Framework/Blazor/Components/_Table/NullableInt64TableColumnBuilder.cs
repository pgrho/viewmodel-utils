namespace Shipwreck.ViewModelUtils.Components;

internal sealed class NullableInt64TableColumnBuilder : PropertyTableColumnBuilder<long?>
{
    protected override PropertyTableColumn<long?> CreateTableColumnCore()
        => new NullableInt64TableColumn();
}
