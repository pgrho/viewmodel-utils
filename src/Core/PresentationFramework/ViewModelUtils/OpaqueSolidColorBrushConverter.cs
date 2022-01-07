using System.Windows.Media;

namespace Shipwreck.ViewModelUtils;

[ValueConversion(typeof(Color), typeof(SolidColorBrush))]
public sealed class OpaqueSolidColorBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is SolidColorBrush b)
        {
            var c = b.Color;
            var nc = Color.FromRgb(
                        (byte)((c.R * c.A + 255 * (255 - c.A)) / 255),
                        (byte)((c.G * c.A + 255 * (255 - c.A)) / 255),
                        (byte)((c.B * c.A + 255 * (255 - c.A)) / 255));
            return new SolidColorBrush(nc);
        }
        return null;
    }

    object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
