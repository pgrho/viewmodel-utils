using System.Collections;

namespace Shipwreck.ViewModelUtils.Components;

public partial class EnumExpressionFormGroup<T> : ExpressionBoundFormGroup<T?>
    where T : struct, Enum
{
    private long? Int64Value
    {
        get => (Value as IConvertible)?.ToInt64(null);
        set
        {
            if (value == null)
            {
                Value = null;
            }
            else
            {
                Value = (T)Enum.ToObject(typeof(T), ((IConvertible)value).ToType(Enum.GetUnderlyingType(typeof(T)), null));
            }
        }
    }

    #region Values

    private static ReadOnlyCollection<T?> _DefaultValues;

    private ReadOnlyCollection<T?> _Values;

    [Parameter]
    public IEnumerable Values
    {
        get => _Values;
        set => SetProperty(ref _Values, Array.AsReadOnly(value?.Cast<T?>().ToArray()));
    }

    protected virtual IEnumerable<T?> GetValues()
    {
        if (_Values != null)
        {
            return _Values;
        }

        if (_DefaultValues == null)
        {
            _DefaultValues = Array.AsReadOnly(Enum.GetValues(typeof(T)).Cast<T?>().ToArray());
        }

        if ((Member as PropertyInfo)?.PropertyType.IsEnum != true)
        {
            return new T?[] { null }.Concat(_DefaultValues);
        }

        return _DefaultValues;
    }

    #endregion Values
}
