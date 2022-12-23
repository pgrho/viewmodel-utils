namespace Shipwreck.ViewModelUtils.Components;

public abstract class IntegerFormGroup<T> : InputFormGroup<T?>
    where T : struct, IEquatable<T>, IComparable<T>, IFormattable, IConvertible
{
    [Parameter]
    public Action<T> NonNullValueChanged { get; set; }

    [Parameter]
    public T NonNullValue
    {
        get => Value ?? default;
        set => Value = value;
    }

    protected override void InvokeValueChanged()
    {
        base.InvokeValueChanged();

        NonNullValueChanged?.Invoke(NonNullValue);
    }
}
