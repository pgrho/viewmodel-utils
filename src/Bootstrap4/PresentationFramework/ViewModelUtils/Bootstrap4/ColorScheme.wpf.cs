using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Shipwreck.ViewModelUtils.Bootstrap4
{
    public partial class ColorScheme
    {
        public static readonly DependencyProperty SchemeProperty
            = DependencyProperty.RegisterAttached("Scheme", typeof(ColorScheme), typeof(ColorScheme), new FrameworkPropertyMetadata(Default)
            {
                Inherits = true
            });

        private static Color GetColor(int bgra)
            => Color.FromArgb((byte)bgra, (byte)(bgra >> 8), (byte)(bgra >> 16), (byte)(bgra >> 24));

        private static SolidColorBrush GetBrush(int bgra)
        {
            var s = new SolidColorBrush(GetColor(bgra));
            s.Freeze();
            return s;
        }

        public static ColorScheme GetScheme(DependencyObject obj) => (ColorScheme)obj.GetValue(SchemeProperty);

        public static void SetScheme(DependencyObject obj, ColorScheme value) => obj.SetValue(SchemeProperty, value);

        #region TextBrush

        private SolidColorBrush _TextBrush;
        public SolidColorBrush TextBrush => _TextBrush ??= GetBrush(TextColor);

        #endregion TextBrush

        #region BackgroundBrush

        private SolidColorBrush _BackgroundBrush;
        public SolidColorBrush BackgroundBrush => _BackgroundBrush ??= GetBrush(BackgroundColor);

        #endregion BackgroundBrush

        #region BorderBrush

        private SolidColorBrush _BorderBrush;
        public SolidColorBrush BorderBrush => _BorderBrush ??= GetBrush(BorderColor);

        #endregion BorderBrush

        #region ActiveTextBrush

        private SolidColorBrush _ActiveTextBrush;
        public SolidColorBrush ActiveTextBrush => _ActiveTextBrush ??= GetBrush(ActiveTextColor);

        #endregion ActiveTextBrush

        #region ActiveBackgroundBrush

        private SolidColorBrush _ActiveBackgroundBrush;
        public SolidColorBrush ActiveBackgroundBrush => _ActiveBackgroundBrush ??= GetBrush(ActiveBackgroundColor);

        #endregion ActiveBackgroundBrush

        #region ActiveBorderBrush

        private SolidColorBrush _ActiveBorderBrush;
        public SolidColorBrush ActiveBorderBrush => _ActiveBorderBrush ??= GetBrush(ActiveBorderColor);

        #endregion ActiveBorderBrush

        //#region HoverBackgroundBrush

        //private SolidColorBrush _HoverBackgroundBrush;
        //public SolidColorBrush HoverBackgroundBrush => _HoverBackgroundBrush ??= GetBrush(HoverBackgroundColor);

        //#endregion HoverBackgroundBrush

        //#region HoverBorderBrush

        //private SolidColorBrush _HoverBorderBrush;
        //public SolidColorBrush HoverBorderBrush => _HoverBorderBrush ??= GetBrush(HoverBorderColor);

        //#endregion HoverBorderBrush

        #region FocusBackgroundBrush

        private SolidColorBrush _FocusBackgroundBrush;
        public SolidColorBrush FocusBackgroundBrush => _FocusBackgroundBrush ??= GetBrush(FocusBackgroundColor);

        #endregion FocusBackgroundBrush

        #region FocusBorderBrush

        private SolidColorBrush _FocusBorderBrush;
        public SolidColorBrush FocusBorderBrush => _FocusBorderBrush ??= GetBrush(FocusBorderColor);

        #endregion FocusBorderBrush

        #region FocusShadowBrush

        private SolidColorBrush _FocusShadowBrush;
        public SolidColorBrush FocusShadowBrush => _FocusShadowBrush ??= GetBrush(FocusShadowColor);

        #endregion FocusShadowBrush

        #region FocusVisualStyle

        private Style _FocusVisualStyle;

        public Style FocusVisualStyle
        {
            get
            {
                if (_FocusVisualStyle == null)
                {
                    _FocusVisualStyle = new Style(typeof(Control));

                    const double width = 3.2;

                    var ct = new ControlTemplate();
                    var rect = new FrameworkElementFactory(typeof(Rectangle));
                    rect.SetValue(Rectangle.StrokeThicknessProperty, width);
                    rect.SetValue(Rectangle.StrokeProperty, FocusShadowBrush);
                    rect.SetValue(Rectangle.StrokeLineJoinProperty, PenLineJoin.Round);
                    ct.VisualTree = rect;
                    ct.Seal();

                    _FocusVisualStyle.Setters.Add(new Setter(Control.TemplateProperty, ct));
                    _FocusVisualStyle.Setters.Add(new Setter(Control.MarginProperty, new Thickness(-width)));
                    _FocusVisualStyle.Seal();
                }
                return _FocusVisualStyle;
            }
        }

        #endregion FocusVisualStyle
    }
}
