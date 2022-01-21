namespace Shipwreck.ViewModelUtils.Components;

public abstract class IntegerFormGroup<T> : InputFormGroup<T>
    where T : struct, IEquatable<T>, IComparable<T>, IFormattable, IConvertible
{
}
