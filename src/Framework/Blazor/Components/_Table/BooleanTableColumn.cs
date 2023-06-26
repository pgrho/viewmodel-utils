namespace Shipwreck.ViewModelUtils.Components;

public sealed class BooleanTableColumn : TableColumn
{
    public Func<object, bool> GetValueDelegate { get; set; }

    public Action<object, bool> SetValueDelegate { get; set; }

    public override void RenderCell(RenderTreeBuilder builder, object dataContext)
    {
        builder.OpenComponent<BooleanCell>(1);
        builder.AddAttribute(2, nameof(BooleanCell.IsEnabled), IsReadOnlyDelegate?.Invoke(dataContext) != true);
        builder.AddAttribute(3, nameof(BooleanCell.IsChanged), IsChangedDelegate?.Invoke(dataContext) == true);
        builder.AddAttribute(4, nameof(BooleanCell.IsChecked), GetValueDelegate?.Invoke(dataContext) == true);
        builder.AddAttribute(5, nameof(BooleanCell.IsCheckedChanged), (Action<bool>)(v => SetValueDelegate(dataContext, v)));
        builder.CloseComponent();
    }
}
