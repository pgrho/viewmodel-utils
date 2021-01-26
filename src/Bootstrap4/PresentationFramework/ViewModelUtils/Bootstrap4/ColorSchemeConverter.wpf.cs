using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace Shipwreck.ViewModelUtils.Bootstrap4
{
    public partial class ColorSchemeConverter : TypeConverter, IValueConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string s && ConvertFromStringCore(s) is var r)
            {
                return r;
            }
            return base.ConvertFrom(context, culture, value);
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
