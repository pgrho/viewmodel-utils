using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Shipwreck.ViewModelUtils
{
    [ValueConversion(typeof(Thickness), typeof(Thickness))]
    public sealed class ThicknessMatrixConverter : IValueConverter
    {
        public double M11 { get; set; }
        public double M12 { get; set; }
        public double M13 { get; set; }
        public double M14 { get; set; }
        public double M21 { get; set; }
        public double M22 { get; set; }
        public double M23 { get; set; }
        public double M24 { get; set; }
        public double M31 { get; set; }
        public double M32 { get; set; }
        public double M33 { get; set; }
        public double M34 { get; set; }
        public double M41 { get; set; }
        public double M42 { get; set; }
        public double M43 { get; set; }
        public double M44 { get; set; }
        public double OffsetLeft { get; set; }
        public double OffsetTop { get; set; }
        public double OffsetRight { get; set; }
        public double OffsetBottom { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var t = value is Thickness a ? a : default;
            return new Thickness(
                t.Left * M11 + t.Top * M21 + t.Right * M31 + t.Bottom * M41 + OffsetLeft,
                t.Left * M12 + t.Top * M22 + t.Right * M32 + t.Bottom * M42 + OffsetTop,
                t.Left * M13 + t.Top * M23 + t.Right * M33 + t.Bottom * M43 + OffsetRight,
                t.Left * M14 + t.Top * M24 + t.Right * M34 + t.Bottom * M44 + OffsetBottom);
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
