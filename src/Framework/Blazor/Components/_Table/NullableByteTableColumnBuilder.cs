namespace Shipwreck.ViewModelUtils.Components;

internal sealed class NullableByteTableColumnBuilder : PropertyTableColumnBuilder<byte?>
{
    protected override PropertyTableColumn<byte?> CreateTableColumnCore()
        => new NullableByteTableColumn();
}
