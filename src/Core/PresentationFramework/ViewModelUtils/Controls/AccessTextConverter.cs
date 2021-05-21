using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Shipwreck.ViewModelUtils.Controls
{
    public sealed class AccessTextConverter : IMultiValueConverter, IValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length > 1)
            {
                var t = values[0]?.ToString();
                var m = values[1]?.ToString()?.FirstOrDefault() ?? default;

                if (!string.IsNullOrEmpty(t))
                {
                    if (m == '\0' || m == '_')
                    {
                        return Escape(t);
                    }
                    //else if (t[0] == m)
                    //{
                    //    return "_" + m + Escape(t.Substring(1));
                    //}
                    else
                    {
                        return Escape(t) + " (_" + m + ")";
                    }
                }
            }
            return null;
        }

        public static string Escape(string s)
            => s?.Replace("_", "__");

        object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => Enumerable.Repeat(Binding.DoNothing, targetTypes.Length).ToArray();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => Escape(value?.ToString());

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => Binding.DoNothing;
    }
}
