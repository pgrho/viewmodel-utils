namespace Shipwreck.ViewModelUtils.Components;

public class EnumTableColumn<T> : TableColumn
    where T : struct, Enum
{
    public Func<object, T> GetValueDelegate { get; set; }

    public Action<object, T> SetValueDelegate { get; set; }
    public Func<object, IEnumerable<T>> OptionsDelegate { get; set; }

    public override void RenderCell(RenderTreeBuilder builder, object dataContext)
    {
        builder.OpenComponent<EnumCell<T>>(1);
        builder.AddAttribute(2, nameof(EnumCell<T>.IsEnabled), IsReadOnlyDelegate?.Invoke(dataContext) != true);
        builder.AddAttribute(3, nameof(EnumCell<T>.IsChanged), IsChangedDelegate?.Invoke(dataContext) == true);
        builder.AddAttribute(4, nameof(EnumCell<T>.Value), GetValueDelegate(dataContext));
        builder.AddAttribute(5, nameof(EnumCell<T>.ValueChanged), (Action<T>)(v => SetValueDelegate(dataContext, v)));
        builder.AddAttribute(6, nameof(EnumCell<T>.Options), OptionsDelegate(dataContext));
        builder.AddAttribute(7, nameof(EnumCell<T>.InputAttributes), AdditionalAttributes);
        builder.CloseComponent();
    }
}
