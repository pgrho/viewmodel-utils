using System;
using System.Globalization;

namespace Shipwreck.ViewModelUtils.Bootstrap4
{
    public partial class ColorSchemeConverter
    {
        private static object ConvertFromString(string s)
            => s switch
            {
                nameof(ColorScheme.Primary) => ColorScheme.Primary,
                nameof(ColorScheme.Secondary) => ColorScheme.Secondary,
                nameof(ColorScheme.Success) => ColorScheme.Success,
                nameof(ColorScheme.Danger) => ColorScheme.Danger,
                nameof(ColorScheme.Warning) => ColorScheme.Warning,
                nameof(ColorScheme.Info) => ColorScheme.Info,
                nameof(ColorScheme.Dark) => ColorScheme.Dark,
                nameof(ColorScheme.OutlinePrimary) => ColorScheme.OutlinePrimary,
                nameof(ColorScheme.OutlineSecondary) => ColorScheme.OutlineSecondary,
                nameof(ColorScheme.OutlineSuccess) => ColorScheme.OutlineSuccess,
                nameof(ColorScheme.OutlineDanger) => ColorScheme.OutlineDanger,
                nameof(ColorScheme.OutlineWarning) => ColorScheme.OutlineWarning,
                nameof(ColorScheme.OutlineInfo) => ColorScheme.OutlineInfo,
                nameof(ColorScheme.OutlineDark) => ColorScheme.OutlineDark,
                nameof(ColorScheme.Link) => ColorScheme.Link,
                _ => null,
            };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var bs = (value as BorderStyle?) ?? default;
            if ((bs & BorderStyle.Primary) != 0)
            {
                return (bs & BorderStyle.Outline) != 0 ? ColorScheme.OutlinePrimary : ColorScheme.Primary;
            }
            if ((bs & BorderStyle.Secondary) != 0)
            {
                return (bs & BorderStyle.Outline) != 0 ? ColorScheme.OutlineSecondary : ColorScheme.Secondary;
            }
            if ((bs & BorderStyle.Success) != 0)
            {
                return (bs & BorderStyle.Outline) != 0 ? ColorScheme.OutlineSuccess : ColorScheme.Success;
            }
            if ((bs & BorderStyle.Danger) != 0)
            {
                return (bs & BorderStyle.Outline) != 0 ? ColorScheme.OutlineDanger : ColorScheme.Danger;
            }
            if ((bs & BorderStyle.Warning) != 0)
            {
                return (bs & BorderStyle.Outline) != 0 ? ColorScheme.OutlineWarning : ColorScheme.Warning;
            }
            if ((bs & BorderStyle.Info) != 0)
            {
                return (bs & BorderStyle.Outline) != 0 ? ColorScheme.OutlineInfo : ColorScheme.Info;
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
    }
}
