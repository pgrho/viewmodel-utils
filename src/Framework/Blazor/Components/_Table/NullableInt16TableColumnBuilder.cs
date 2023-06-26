namespace Shipwreck.ViewModelUtils.Components;

internal sealed class NullableInt16TableColumnBuilder : PropertyTableColumnBuilder<short?>
{
    protected override PropertyTableColumn<short?> CreateTableColumnCore()
        => new NullableInt16TableColumn();
}
