namespace Shipwreck.ViewModelUtils.Components;

public class Int16TableColumn : PropertyTableColumn<short>
{
    private sealed class Int16Cell : ValueTypeCell<short>
    {
        protected override bool TryParse(string s, out short result)
        {
            if (short.TryParse(s, out var i))
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
        builder.OpenComponent<Int16Cell>(seq++);
        AddAttributes(builder, dataContext, ref seq);
        builder.CloseComponent();
    }
}
