namespace Shipwreck.ViewModelUtils.Components;

internal sealed class NullableInt32TableColumnBuilder : PropertyTableColumnBuilder<int?>
{
    protected override PropertyTableColumn<int?> CreateTableColumnCore()
        => new NullableInt32TableColumn();
}
