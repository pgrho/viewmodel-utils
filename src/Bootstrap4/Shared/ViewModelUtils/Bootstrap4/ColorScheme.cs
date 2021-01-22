#if IS_XAMARIN_FORMS

using Xamarin.Forms;

#elif IS_WPF
using System.ComponentModel;
using System.Windows.Data;
#endif

namespace Shipwreck.ViewModelUtils.Bootstrap4
{
    [TypeConverter(typeof(ColorSchemeConverter))]
    public partial class ColorScheme
    {
        public static readonly ColorScheme Default = new ColorScheme()
        {
            TextColor = FromRgba(33, 37, 41, 255),
            BackgroundColor = FromRgba(0, 0, 0, 0),
            BorderColor = FromRgba(0, 0, 0, 0),
            ActiveTextColor = FromRgba(33, 37, 41, 255),
            ActiveBackgroundColor = FromRgba(0, 0, 0, 0),
            ActiveBorderColor = FromRgba(0, 0, 0, 0),
            FocusBackgroundColor = FromRgba(0, 0, 0, 0),
            FocusBorderColor = FromRgba(0, 0, 0, 0),
            FocusShadowColor = FromRgba(0, 123, 255, 64),
        };

        public static readonly ColorScheme Primary = new ColorScheme()
        {
            TextColor = FromRgba(255, 255, 255, 255),
            BackgroundColor = FromRgba(0, 123, 255, 255),
            BorderColor = FromRgba(0, 123, 255, 255),
            ActiveTextColor = FromRgba(255, 255, 255, 255),
            ActiveBackgroundColor = FromRgba(0, 98, 204, 255),
            ActiveBorderColor = FromRgba(0, 92, 191, 255),
            FocusBackgroundColor = FromRgba(0, 105, 217, 255),
            FocusBorderColor = FromRgba(0, 98, 204, 255),
            FocusShadowColor = FromRgba(38, 143, 255, 128),
        };

        public static readonly ColorScheme OutlinePrimary = new ColorScheme()
        {
            TextColor = FromRgba(0, 123, 255, 255),
            BackgroundColor = FromRgba(0, 0, 0, 0),
            BorderColor = FromRgba(0, 123, 255, 255),
            ActiveTextColor = FromRgba(255, 255, 255, 255),
            ActiveBackgroundColor = FromRgba(0, 123, 255, 255),
            ActiveBorderColor = FromRgba(0, 123, 255, 255),
            FocusBackgroundColor = FromRgba(0, 0, 0, 0),
            FocusBorderColor = FromRgba(0, 123, 255, 255),
            FocusShadowColor = FromRgba(0, 123, 255, 128),
        };

        public static readonly ColorScheme Secondary = new ColorScheme()
        {
            TextColor = FromRgba(255, 255, 255, 255),
            BackgroundColor = FromRgba(108, 117, 125, 255),
            BorderColor = FromRgba(108, 117, 125, 255),
            ActiveTextColor = FromRgba(255, 255, 255, 255),
            ActiveBackgroundColor = FromRgba(84, 91, 98, 255),
            ActiveBorderColor = FromRgba(78, 85, 91, 255),
            FocusBackgroundColor = FromRgba(90, 98, 104, 255),
            FocusBorderColor = FromRgba(84, 91, 98, 255),
            FocusShadowColor = FromRgba(130, 138, 145, 128),
        };

        public static readonly ColorScheme OutlineSecondary = new ColorScheme()
        {
            TextColor = FromRgba(108, 117, 125, 255),
            BackgroundColor = FromRgba(0, 0, 0, 0),
            BorderColor = FromRgba(108, 117, 125, 255),
            ActiveTextColor = FromRgba(255, 255, 255, 255),
            ActiveBackgroundColor = FromRgba(108, 117, 125, 255),
            ActiveBorderColor = FromRgba(108, 117, 125, 255),
            FocusBackgroundColor = FromRgba(0, 0, 0, 0),
            FocusBorderColor = FromRgba(108, 117, 125, 255),
            FocusShadowColor = FromRgba(108, 117, 125, 128),
        };

        public static readonly ColorScheme Success = new ColorScheme()
        {
            TextColor = FromRgba(255, 255, 255, 255),
            BackgroundColor = FromRgba(40, 167, 69, 255),
            BorderColor = FromRgba(40, 167, 69, 255),
            ActiveTextColor = FromRgba(255, 255, 255, 255),
            ActiveBackgroundColor = FromRgba(30, 126, 52, 255),
            ActiveBorderColor = FromRgba(28, 116, 48, 255),
            FocusBackgroundColor = FromRgba(33, 136, 56, 255),
            FocusBorderColor = FromRgba(30, 126, 52, 255),
            FocusShadowColor = FromRgba(72, 180, 97, 128),
        };

        public static readonly ColorScheme OutlineSuccess = new ColorScheme()
        {
            TextColor = FromRgba(40, 167, 69, 255),
            BackgroundColor = FromRgba(0, 0, 0, 0),
            BorderColor = FromRgba(40, 167, 69, 255),
            ActiveTextColor = FromRgba(255, 255, 255, 255),
            ActiveBackgroundColor = FromRgba(40, 167, 69, 255),
            ActiveBorderColor = FromRgba(40, 167, 69, 255),
            FocusBackgroundColor = FromRgba(0, 0, 0, 0),
            FocusBorderColor = FromRgba(40, 167, 69, 255),
            FocusShadowColor = FromRgba(40, 167, 69, 128),
        };

        public static readonly ColorScheme Danger = new ColorScheme()
        {
            TextColor = FromRgba(255, 255, 255, 255),
            BackgroundColor = FromRgba(220, 53, 69, 255),
            BorderColor = FromRgba(220, 53, 69, 255),
            ActiveTextColor = FromRgba(255, 255, 255, 255),
            ActiveBackgroundColor = FromRgba(189, 33, 48, 255),
            ActiveBorderColor = FromRgba(178, 31, 45, 255),
            FocusBackgroundColor = FromRgba(200, 35, 51, 255),
            FocusBorderColor = FromRgba(189, 33, 48, 255),
            FocusShadowColor = FromRgba(225, 83, 97, 128),
        };

        public static readonly ColorScheme OutlineDanger = new ColorScheme()
        {
            TextColor = FromRgba(220, 53, 69, 255),
            BackgroundColor = FromRgba(0, 0, 0, 0),
            BorderColor = FromRgba(220, 53, 69, 255),
            ActiveTextColor = FromRgba(255, 255, 255, 255),
            ActiveBackgroundColor = FromRgba(220, 53, 69, 255),
            ActiveBorderColor = FromRgba(220, 53, 69, 255),
            FocusBackgroundColor = FromRgba(0, 0, 0, 0),
            FocusBorderColor = FromRgba(220, 53, 69, 255),
            FocusShadowColor = FromRgba(220, 53, 69, 128),
        };

        public static readonly ColorScheme Warning = new ColorScheme()
        {
            TextColor = FromRgba(33, 37, 41, 255),
            BackgroundColor = FromRgba(255, 193, 7, 255),
            BorderColor = FromRgba(255, 193, 7, 255),
            ActiveTextColor = FromRgba(33, 37, 41, 255),
            ActiveBackgroundColor = FromRgba(211, 158, 0, 255),
            ActiveBorderColor = FromRgba(198, 149, 0, 255),
            FocusBackgroundColor = FromRgba(224, 168, 0, 255),
            FocusBorderColor = FromRgba(211, 158, 0, 255),
            FocusShadowColor = FromRgba(222, 170, 12, 128),
        };

        public static readonly ColorScheme OutlineWarning = new ColorScheme()
        {
            TextColor = FromRgba(255, 193, 7, 255),
            BackgroundColor = FromRgba(0, 0, 0, 0),
            BorderColor = FromRgba(255, 193, 7, 255),
            ActiveTextColor = FromRgba(33, 37, 41, 255),
            ActiveBackgroundColor = FromRgba(255, 193, 7, 255),
            ActiveBorderColor = FromRgba(255, 193, 7, 255),
            FocusBackgroundColor = FromRgba(0, 0, 0, 0),
            FocusBorderColor = FromRgba(255, 193, 7, 255),
            FocusShadowColor = FromRgba(255, 193, 7, 128),
        };

        public static readonly ColorScheme Info = new ColorScheme()
        {
            TextColor = FromRgba(255, 255, 255, 255),
            BackgroundColor = FromRgba(23, 162, 184, 255),
            BorderColor = FromRgba(23, 162, 184, 255),
            ActiveTextColor = FromRgba(255, 255, 255, 255),
            ActiveBackgroundColor = FromRgba(17, 122, 139, 255),
            ActiveBorderColor = FromRgba(16, 112, 127, 255),
            FocusBackgroundColor = FromRgba(19, 132, 150, 255),
            FocusBorderColor = FromRgba(17, 122, 139, 255),
            FocusShadowColor = FromRgba(58, 176, 195, 128),
        };

        public static readonly ColorScheme OutlineInfo = new ColorScheme()
        {
            TextColor = FromRgba(23, 162, 184, 255),
            BackgroundColor = FromRgba(0, 0, 0, 0),
            BorderColor = FromRgba(23, 162, 184, 255),
            ActiveTextColor = FromRgba(255, 255, 255, 255),
            ActiveBackgroundColor = FromRgba(23, 162, 184, 255),
            ActiveBorderColor = FromRgba(23, 162, 184, 255),
            FocusBackgroundColor = FromRgba(0, 0, 0, 0),
            FocusBorderColor = FromRgba(23, 162, 184, 255),
            FocusShadowColor = FromRgba(23, 162, 184, 128),
        };

        public static readonly ColorScheme Dark = new ColorScheme()
        {
            TextColor = FromRgba(255, 255, 255, 255),
            BackgroundColor = FromRgba(52, 58, 64, 255),
            BorderColor = FromRgba(52, 58, 64, 255),
            ActiveTextColor = FromRgba(255, 255, 255, 255),
            ActiveBackgroundColor = FromRgba(29, 33, 36, 255),
            ActiveBorderColor = FromRgba(23, 26, 29, 255),
            FocusBackgroundColor = FromRgba(35, 39, 43, 255),
            FocusBorderColor = FromRgba(29, 33, 36, 255),
            FocusShadowColor = FromRgba(82, 88, 93, 128),
        };

        public static readonly ColorScheme OutlineDark = new ColorScheme()
        {
            TextColor = FromRgba(52, 58, 64, 255),
            BackgroundColor = FromRgba(0, 0, 0, 0),
            BorderColor = FromRgba(52, 58, 64, 255),
            ActiveTextColor = FromRgba(255, 255, 255, 255),
            ActiveBackgroundColor = FromRgba(52, 58, 64, 255),
            ActiveBorderColor = FromRgba(52, 58, 64, 255),
            FocusBackgroundColor = FromRgba(0, 0, 0, 0),
            FocusBorderColor = FromRgba(52, 58, 64, 255),
            FocusShadowColor = FromRgba(52, 58, 64, 128),
        };

        public static readonly ColorScheme Link = new ColorScheme()
        {
            TextColor = FromRgba(0, 123, 255, 255),
            BackgroundColor = FromRgba(0, 0, 0, 0),
            BorderColor = FromRgba(0, 0, 0, 0),
            ActiveTextColor = FromRgba(0, 123, 255, 255),
            ActiveBackgroundColor = FromRgba(0, 0, 0, 0),
            ActiveBorderColor = FromRgba(0, 0, 0, 0),
            FocusBackgroundColor = FromRgba(0, 0, 0, 0),
            FocusBorderColor = FromRgba(0, 0, 0, 0),
            FocusShadowColor = FromRgba(0, 123, 255, 64),
        };

        public int TextColor { get; private set; }
        public int BackgroundColor { get; private set; }
        public int BorderColor { get; private set; }
        public int ActiveTextColor { get; private set; }
        public int ActiveBackgroundColor { get; private set; }
        public int ActiveBorderColor { get; private set; }

        //public int HoverBackgroundColor { get; private set; }
        //public int HoverBorderColor { get; private set; }
        public int FocusBackgroundColor { get; private set; }

        public int FocusBorderColor { get; private set; }
        public int FocusShadowColor { get; private set; }

        private static int FromRgba(int r, int g, int b, int a)
             => (b << 24) | (g << 16) | (r << 8) | a;
    }
}
