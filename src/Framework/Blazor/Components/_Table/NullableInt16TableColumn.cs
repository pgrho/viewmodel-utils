namespace Shipwreck.ViewModelUtils.Components;

public class NullableInt16TableColumn : PropertyTableColumn<short?>
{
    private sealed class NullableInt16Cell : NullableValueTypeCell<short>
    {
        protected override bool TryParse(string s, out short? result)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                result = null;
                return true;
            }
            if (short.TryParse(s, out var i))
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
        builder.OpenComponent<NullableInt16Cell>(seq++);
        AddAttributes(builder, dataContext, ref seq);
        builder.CloseComponent();
    }
}
