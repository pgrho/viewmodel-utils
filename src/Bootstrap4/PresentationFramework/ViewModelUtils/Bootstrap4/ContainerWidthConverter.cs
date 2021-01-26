using System;
using System.Globalization;
using System.Windows.Data;

namespace Shipwreck.ViewModelUtils.Bootstrap4
{
    public sealed class ContainerWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IConvertible c)
            {
                var w = c.ToDouble(culture);
                return w switch
                {
                    double d when d < 576 => Math.Min(Math.Max(d, 0), 540),
                    double d when d < 768 => 540,
                    double d when d < 992 => 720,
                    double d when d < 1200 => 960,
                    double d when d < 1400 => 1140,
                    _ => 1320
                };
            }
            return 0;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
