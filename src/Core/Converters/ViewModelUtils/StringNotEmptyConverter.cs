using System;
using System.Globalization;

namespace Shipwreck.ViewModelUtils
{
    public sealed class StringNotEmptyConverter : BooleanConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => ToResult(((value as string) ?? value?.ToString())?.Length > 0, targetType, culture);
    }
}
