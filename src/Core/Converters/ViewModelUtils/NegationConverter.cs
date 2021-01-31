using System;
using System.Globalization;
#if IS_WPF
using System.Windows;
using System.Windows.Data;
#elif IS_XAMARIN_FORMS
using Xamarin.Forms;
#endif

namespace Shipwreck.ViewModelUtils
{
#if IS_WPF
    [ValueConversion(typeof(string), typeof(string))]
    [ValueConversion(typeof(bool), typeof(Visibility))]
#endif
    public sealed class NegationConverter : BooleanConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => ToResult(value is bool b ? b : value is IConvertible c ? c.ToBoolean(culture) : value == null, targetType, culture);
    }
}
