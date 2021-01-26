using System;
using System.Globalization;

namespace Shipwreck.ViewModelUtils
{
    public sealed class HasFlagConverter : BooleanConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = (value as IConvertible)?.ToInt64(culture);
            var f = (parameter as IConvertible)?.ToInt64(culture);

            return ToResult(v != null && f != null && (v.Value & f.Value) == f.Value, targetType, culture);
        }
    }
}
