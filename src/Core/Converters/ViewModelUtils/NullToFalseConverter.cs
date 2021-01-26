using System;
using System.Globalization;
#if IS_WPF
using System.Windows;
using System.Windows.Data;
#elif IS_XAMARIN_FORMS
#endif

namespace Shipwreck.ViewModelUtils
{
    public sealed class NullToFalseConverter : BooleanConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => ToResult(value != null, targetType, culture);
    }
}
