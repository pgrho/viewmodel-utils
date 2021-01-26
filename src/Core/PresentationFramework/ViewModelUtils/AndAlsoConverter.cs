using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Shipwreck.ViewModelUtils
{
    public sealed class AndAlsoConverter : IMultiValueConverter
    {
        public object TruePart { get; set; } = DependencyProperty.UnsetValue;
        public object FalsePart { get; set; } = DependencyProperty.UnsetValue;

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var v = values.All(e => e != null && (!(e is bool b) || b));
            return BooleanConverterBase.ToResultCore(v, v ? TruePart : FalsePart, targetType, culture);
        }

        object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
