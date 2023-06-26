namespace Shipwreck.ViewModelUtils.Components;

public class StringTableColumn : PropertyTableColumn<string>
{
    public override void RenderCell(RenderTreeBuilder builder, object dataContext)
    {
        var seq = 1;
        builder.OpenComponent<StringCell>(seq++);
        AddAttributes(builder, dataContext, ref seq);
        builder.CloseComponent();
    }
}
