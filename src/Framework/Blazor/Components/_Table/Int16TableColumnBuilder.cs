namespace Shipwreck.ViewModelUtils.Components;

internal sealed class Int16TableColumnBuilder : PropertyTableColumnBuilder<short>
{
    protected override PropertyTableColumn<short> CreateTableColumnCore()
        => new Int16TableColumn();
}
