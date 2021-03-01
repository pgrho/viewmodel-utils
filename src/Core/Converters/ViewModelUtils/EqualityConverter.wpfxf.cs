using System;
using System.Globalization;

namespace Shipwreck.ViewModelUtils
{
    public sealed partial class EqualityConverter : BooleanConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => ToResult(Equals(value, parameter), targetType, culture);
    }
}
