using System;
using System.Globalization;

using Xamarin.Forms;

namespace Shipwreck.ViewModelUtils.Bootstrap4
{
    public partial class ColorSchemeConverter : TypeConverter, IValueConverter
    {
        public override object ConvertFromInvariantString(string value)
            => ConvertFromString(value) ?? base.ConvertFromInvariantString(value);

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
