namespace Shipwreck.ViewModelUtils.Components;

public class EntitySelectorTableColumn : TableColumn
{
    public Func<object, IEntitySelector> GetValueDelegate { get; set; }

    public override void RenderCell(RenderTreeBuilder builder, object dataContext)
    {
        builder.OpenComponent<EntitySelectorCell>(1);
        builder.AddAttribute(2, nameof(EntitySelectorCell.DataContext), GetValueDelegate?.Invoke(dataContext));
        builder.AddAttribute(3, nameof(EntitySelectorCell.IsChanged), IsChangedDelegate?.Invoke(dataContext) == true);
        builder.CloseComponent();
    }
}
