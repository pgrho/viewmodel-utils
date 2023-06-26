namespace Shipwreck.ViewModelUtils.Components;

internal sealed class StringTableColumnBuilder : PropertyTableColumnBuilder<string>
{
    protected override PropertyTableColumn<string> CreateTableColumnCore()
        => new StringTableColumn();
}
