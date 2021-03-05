using System;
using System.Globalization;
#if IS_WPF
using System.Windows.Data;
#elif IS_XAMARIN_FORMS
using Xamarin.Forms;
#endif

namespace Shipwreck.ViewModelUtils
{
#if IS_WPF
    [ValueConversion(typeof(Enum), typeof(string))]
#endif
    public class EnumGroupNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value is Enum ? EnumDataAnnotations.Get(value.GetType()).GetGroupName(value) : value;

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
