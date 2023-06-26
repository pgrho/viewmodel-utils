namespace Shipwreck.ViewModelUtils.Components;

public class Int64TableColumn : PropertyTableColumn<long>
{
    private sealed class Int64Cell : ValueTypeCell<long>
    {
        protected override bool TryParse(string s, out long result)
        {
            if (long.TryParse(s, out var i))
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
        builder.OpenComponent<Int64Cell>(seq++);
        AddAttributes(builder, dataContext, ref seq);
        builder.CloseComponent();
    }
}
