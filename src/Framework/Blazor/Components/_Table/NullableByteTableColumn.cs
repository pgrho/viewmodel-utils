namespace Shipwreck.ViewModelUtils.Components;

public class NullableByteTableColumn : PropertyTableColumn<byte?>
{
    private sealed class NullableByteCell : NullableValueTypeCell<byte>
    {
        protected override bool TryParse(string s, out byte? result)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                result = null;
                return true;
            }
            if (byte.TryParse(s, out var i))
            {
                result = i;
                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }
    }

    public override void RenderCell(RenderTreeBuilder builder, object dataContext)
    {
        var seq = 1;
        builder.OpenComponent<NullableByteCell>(seq++);
        AddAttributes(builder, dataContext, ref seq);
        builder.CloseComponent();
    }
}
