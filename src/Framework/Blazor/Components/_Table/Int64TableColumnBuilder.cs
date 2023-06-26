namespace Shipwreck.ViewModelUtils.Components;

internal sealed class Int64TableColumnBuilder : PropertyTableColumnBuilder<long>
{
    protected override PropertyTableColumn<long> CreateTableColumnCore()
        => new Int64TableColumn();
}
