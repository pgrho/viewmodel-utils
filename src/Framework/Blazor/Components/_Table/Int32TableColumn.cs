namespace Shipwreck.ViewModelUtils.Components;

public class Int32TableColumn : PropertyTableColumn<int>
{
    private sealed class Int32Cell : ValueTypeCell<int>
    {
        protected override bool TryParse(string s, out int result)
        {
            if (int.TryParse(s, out var i))
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
        builder.OpenComponent<Int32Cell>(seq++);
        AddAttributes(builder, dataContext, ref seq);
        builder.CloseComponent();
    }
}
