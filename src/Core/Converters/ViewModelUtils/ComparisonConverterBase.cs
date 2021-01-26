using System;
using System.Globalization;

namespace Shipwreck.ViewModelUtils
{
    public abstract partial class ComparisonConverterBase : BooleanConverterBase
    {
        public object Operand { get; set; }

        public sealed override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => ToResult(
                value is string s ? ToBoolean(culture.CompareInfo.Compare(s, (parameter ?? Operand)?.ToString() ?? string.Empty))
                : value is IComparable c && (parameter ?? Operand) is IConvertible pc ? ToBoolean(c.CompareTo(pc.ToType(c.GetType(), culture)))
                : false,
                targetType,
                culture);

        protected abstract bool ToBoolean(int sign);
    }
}
