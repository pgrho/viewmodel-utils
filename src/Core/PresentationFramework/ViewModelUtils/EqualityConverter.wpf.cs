using System;
using System.Globalization;
using System.Windows.Data;

namespace Shipwreck.ViewModelUtils
{
    public partial class EqualityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            for (var i = 1; i < values.Length; i++)
            {
                if (!Equals(values[0], values[i]))
                {
                    return ToResult(false, targetType, culture);
                }
            }
            return ToResult(true, targetType, culture);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
