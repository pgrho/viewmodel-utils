namespace Shipwreck.ViewModelUtils.Components;

public class NullableInt64TableColumn : PropertyTableColumn<long?>
{
    private sealed class NullableInt64Cell : NullableValueTypeCell<long>
    {
        protected override bool TryParse(string s, out long? result)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                result = null;
                return true;
            }
            if (long.TryParse(s, out var i))
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
        builder.OpenComponent<NullableInt64Cell>(seq++);
        AddAttributes(builder, dataContext, ref seq);
        builder.CloseComponent();
    }
}
