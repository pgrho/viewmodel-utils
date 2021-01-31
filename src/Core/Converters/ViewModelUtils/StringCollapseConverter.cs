using System;
using System.Globalization;
using System.Text.RegularExpressions;
#if IS_WPF
using System.Windows.Data;
#elif IS_XAMARIN_FORMS
using Xamarin.Forms;
#endif

namespace Shipwreck.ViewModelUtils
{
#if IS_WPF
    [ValueConversion(typeof(string), typeof(string))]
#endif
    public sealed class StringCollapseConverter : IValueConverter
    {
        private string _Pattern = @"[\r\n\t\s]+";

        public string Pattern
        {
            get => _Pattern;
            set
            {
                if (value != _Pattern)
                {
                    _Pattern = value;
                    _Regex = null;
                }
            }
        }

        private Regex _Regex;

        private Regex Regex
            => _Regex ??= (_Pattern != null ? new Regex(_Pattern) : null);

        public string Replacement { get; set; } = " ";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var s = value?.ToString();
            return s == null || Regex == null ? s : Regex.Replace(s, Replacement);
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
