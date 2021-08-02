using System;
using System.Globalization;
using System.Windows.Data;

namespace Shipwreck.ViewModelUtils.Controls
{
    internal sealed class DateTimeYearConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => (value as DateTime?)?.Year.ToString("D");

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => value is string s ? string.IsNullOrWhiteSpace(s) ? (DateTime?)null : int.TryParse(s, out var y) ? new DateTime(y, 1, 1) : DateTime.Parse(s)
            : ((IConvertible)value)?.ToDateTime(culture);
    }
}
