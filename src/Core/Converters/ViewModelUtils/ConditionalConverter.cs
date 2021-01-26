using System;
using System.Globalization;

namespace Shipwreck.ViewModelUtils
{
    public sealed class ConditionalConverter : BooleanConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => ToResult((value is bool b) && b, targetType, culture);
    }
}
