namespace Shipwreck.ViewModelUtils.Components;

public class ReadOnlyTableColumn : TableColumn
{
    public Func<object, object> GetValueDelegate { get; set; }

    public override void RenderCell(RenderTreeBuilder builder, object dataContext)
    {
        builder.OpenElement(1, "td");

        var v = GetValueDelegate?.Invoke(dataContext);

        builder.AddContent(2, v?.ToString());

        builder.CloseElement();
    }
}
