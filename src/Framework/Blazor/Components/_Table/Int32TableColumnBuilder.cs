namespace Shipwreck.ViewModelUtils.Components;

internal sealed class Int32TableColumnBuilder : PropertyTableColumnBuilder<int>
{
    protected override PropertyTableColumn<int> CreateTableColumnCore()
        => new Int32TableColumn();
}
