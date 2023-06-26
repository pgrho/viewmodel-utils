namespace Shipwreck.ViewModelUtils.Components;

partial class NullableValueTypeCell<T>
    where T : struct, IFormattable, IComparable, IComparable<T>, IEquatable<T>, IConvertible
{
    protected override string ToString(T? value)
        => value?.ToString("D", CultureInfo.InvariantCulture);

    protected override bool TryParse(string s, out T? result)
    {
        if (string.IsNullOrWhiteSpace(s))
        {
            result = null;
            return true;
        }
        try
        {
            result = (T?)((IConvertible)s).ToType(typeof(T), CultureInfo.InvariantCulture);
            return true;
        }
        catch
        {
        }
        result = null;
        return false;
    }
}
