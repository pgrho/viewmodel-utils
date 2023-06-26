namespace Shipwreck.ViewModelUtils.Components;

internal sealed class ByteTableColumnBuilder : PropertyTableColumnBuilder<byte>
{
    protected override PropertyTableColumn<byte> CreateTableColumnCore()
        => new ByteTableColumn();
}
