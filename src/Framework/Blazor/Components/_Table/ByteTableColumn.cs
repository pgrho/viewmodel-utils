namespace Shipwreck.ViewModelUtils.Components;

public class ByteTableColumn : PropertyTableColumn<byte>
{
    private sealed class ByteCell : ValueTypeCell<byte>
    {
        protected override bool TryParse(string s, out byte result)
        {
            if (byte.TryParse(s, out var i))
            {
                result = i;
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }
    }

    public override void RenderCell(RenderTreeBuilder builder, object dataContext)
    {
        var seq = 1;
        builder.OpenComponent<ByteCell>(seq++);
        AddAttributes(builder, dataContext, ref seq);
        builder.CloseComponent();
    }
}
