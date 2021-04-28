using System;
using System.Globalization;
using System.Windows.Data;

namespace Shipwreck.ViewModelUtils.Controls
{
    internal sealed class EntitySelectorDisplayTextConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length > 1 && values[1] is IEntitySelector sel)
            {
                return sel.GetDisplayText(values[0]);
            }
            return null;
        }

        object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
