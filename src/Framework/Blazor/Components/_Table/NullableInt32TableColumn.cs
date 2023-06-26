namespace Shipwreck.ViewModelUtils.Components;

public class NullableInt32TableColumn : PropertyTableColumn<int?>
{
    private sealed class NullableInt32Cell : NullableValueTypeCell<int>
    {
        protected override bool TryParse(string s, out int? result)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                result = null;
                return true;
            }
            if (int.TryParse(s, out var i))
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
        builder.OpenComponent<NullableInt32Cell>(seq++);
        AddAttributes(builder, dataContext, ref seq);
        builder.CloseComponent();
    }
}
