using System;
using System.Globalization;
using Shipwreck.BootstrapControls;

#if IS_WPF
using System.Windows.Data;
#elif IS_XAMARIN_FORMS
using Xamarin.Forms;
#endif

namespace Shipwreck.ViewModelUtils
{
#if IS_WPF
    [ValueConversion(typeof(BorderStyle), typeof(ColorScheme))]
#endif
    public sealed partial class BorderStyleToColorSchemeConverter : IValueConverter
    {
        public BorderStyle? DefaultStyle { get; set; }

        public bool ForceOutline { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var bs = value is BorderStyle a ? a : default;
            if (bs == DefaultStyle
                || (ForceOutline && (bs | BorderStyle.Outline) == DefaultStyle))
            {
                return null;
            }
            if ((bs & BorderStyle.Primary) != 0)
            {
                return ForceOutline || (bs & BorderStyle.Outline) != 0 ? ColorScheme.OutlinePrimary : ColorScheme.Primary;
            }
            if ((bs & BorderStyle.Secondary) != 0)
            {
                return ForceOutline || (bs & BorderStyle.Outline) != 0 ? ColorScheme.OutlineSecondary : ColorScheme.Secondary;
            }
            if ((bs & BorderStyle.Success) != 0)
            {
                return ForceOutline || (bs & BorderStyle.Outline) != 0 ? ColorScheme.OutlineSuccess : ColorScheme.Success;
            }
            if ((bs & BorderStyle.Danger) != 0)
            {
                return ForceOutline || (bs & BorderStyle.Outline) != 0 ? ColorScheme.OutlineDanger : ColorScheme.Danger;
            }
            if ((bs & BorderStyle.Warning) != 0)
            {
                return ForceOutline || (bs & BorderStyle.Outline) != 0 ? ColorScheme.OutlineWarning : ColorScheme.Warning;
            }
            if ((bs & BorderStyle.Info) != 0)
            {
                return ForceOutline || (bs & BorderStyle.Outline) != 0 ? ColorScheme.OutlineInfo : ColorScheme.Info;
            }
            //if ((bs & BorderStyle.Light) != 0)
            //{
            //    return (bs & BorderStyle.Outline) != 0 ? ColorScheme.OutlineLight : ColorScheme.Light;
            //}
            //if ((bs & BorderStyle.Dark) != 0)
            //{
            //    return (bs & BorderStyle.Outline) != 0 ? ColorScheme.OutlineDark : ColorScheme.Dark;
            //}
            if ((bs & BorderStyle.Link) != 0)
            {
                return ColorScheme.Link;
            }
            return ColorScheme.OutlineSecondary;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
