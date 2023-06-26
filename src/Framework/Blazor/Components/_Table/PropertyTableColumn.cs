namespace Shipwreck.ViewModelUtils.Components;

public abstract class PropertyTableColumn<T> : TableColumn
{
    public Dictionary<string, object> InputAttributes { get; set; }

    public Func<object, T> GetValueDelegate { get; set; }

    public Action<object, T> SetValueDelegate { get; set; }
    public Func<object, IEnumerable<T>> OptionsDelegate { get; set; }

    protected void AddAttributes(RenderTreeBuilder builder, object dataContext, ref int sequence)
    {
        builder.AddAttribute(sequence++, nameof(StringCell.IsEnabled), IsReadOnlyDelegate?.Invoke(dataContext) != true);
        builder.AddAttribute(sequence++, nameof(StringCell.IsChanged), IsChangedDelegate?.Invoke(dataContext) == true);
        builder.AddAttribute(sequence++, nameof(StringCell.Value), GetValueDelegate(dataContext));
        builder.AddAttribute(sequence++, nameof(StringCell.ValueChanged), (Action<T>)(v => SetValueDelegate(dataContext, v)));
        builder.AddAttribute(sequence++, nameof(StringCell.InputAttributes), InputAttributes);
    }
}
