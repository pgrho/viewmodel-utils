namespace Shipwreck.ViewModelUtils.Components;

public sealed class TemplateTableColumn : TableColumn
{
    private readonly Type _ComponentType;

    public TemplateTableColumn(Type componentType)
    {
        _ComponentType = componentType;
    }

    public override void RenderCell(RenderTreeBuilder builder, object dataContext)
    {
        builder.OpenElement(0, "td");
        if (IsChangedDelegate?.Invoke(dataContext) == true)
        {
            builder.AddAttribute(1, "class", "table-danger");
        }
        builder.OpenComponent(2, _ComponentType);
        builder.AddAttribute(3, nameof(IBindableComponent.DataContext), dataContext);
        builder.CloseComponent();
        builder.CloseElement();
    }
}
