using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Shipwreck.ViewModelUtils.FontAwesome5
{
    public sealed class FontAwesomeTextBlock : TextBlock
    {
        static FontAwesomeTextBlock()
        {
            RenderTransformOriginProperty.OverrideMetadata(typeof(FontAwesomeTextBlock), new FrameworkPropertyMetadata(new Point(0.5, 0.5)));
        }

        #region Icon

        public static readonly DependencyProperty IconProperty
            = DependencyProperty.RegisterAttached(nameof(Icon), typeof(string), typeof(FontAwesomeTextBlock), new FrameworkPropertyMetadata(null, OnIconPropertyChanged));

        public string Icon
        {
            get => (string)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public static string GetIcon(TextElement obj)
            => (string)obj.GetValue(IconProperty);

        public static void SetIcon(TextElement obj, string value)
            => obj.SetValue(IconProperty, value);

        #endregion Icon

        #region SpinAnimation

        private static DoubleAnimationUsingKeyFrames _SpinAnimation;

        private static DoubleAnimationUsingKeyFrames SpinAnimation
        {
            get
            {
                if (_SpinAnimation == null)
                {
                    var da = _SpinAnimation = new DoubleAnimationUsingKeyFrames();
                    da.KeyFrames.Add(new LinearDoubleKeyFrame(0, TimeSpan.Zero));
                    da.KeyFrames.Add(new LinearDoubleKeyFrame(360, TimeSpan.FromSeconds(1)));
                    da.RepeatBehavior = RepeatBehavior.Forever;
                    da.Freeze();
                }
                return _SpinAnimation;
            }
        }

        #endregion SpinAnimation

        #region PulseAnimation

        private static DoubleAnimationUsingKeyFrames _PulseAnimation;

        private static DoubleAnimationUsingKeyFrames PulseAnimation
        {
            get
            {
                if (_PulseAnimation == null)
                {
                    var da = _PulseAnimation = new DoubleAnimationUsingKeyFrames();
                    for (var i = 0; i <= 8; i++)
                    {
                        da.KeyFrames.Add(new DiscreteDoubleKeyFrame(45 * i, TimeSpan.FromMilliseconds(125 * i)));
                    }
                    da.RepeatBehavior = RepeatBehavior.Forever;
                    da.Freeze();
                }
                return _PulseAnimation;
            }
        }

        #endregion PulseAnimation

        private static ResourceDictionary _Fonts;
        private static FontFamily _SolidFontFamily;
        private static FontFamily _RegularFontFamily;

        private static void OnIconPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBlock b)
            {
                if (e.NewValue is string s && FontAwesome.TryParse(s, out var f, out var t, out var a))
                {
                    _Fonts ??= new ResourceDictionary
                    {
                        Source = System.IO.Packaging.PackUriHelper.Create(
                            new Uri("application:///"),
                            new Uri("/Shipwreck.ViewModelUtils.FontAwesome5.PresentationFramework;component/Resources/Fonts.xaml", UriKind.Relative)
                        )
                    };
                    b.FontFamily = f == "fa-regular-400" ? (_RegularFontFamily ??= _Fonts["FontAwesomeRegular"] as FontFamily) : (_SolidFontFamily ??= _Fonts["FontAwesomeSolid"] as FontFamily);
                    b.Text = t;
                    switch (a)
                    {
                        case FontAwesomeAnimation.Spin:
                            var st = new RotateTransform();
                            st.BeginAnimation(RotateTransform.AngleProperty, SpinAnimation);
                            b.RenderTransform = st;
                            break;

                        case FontAwesomeAnimation.Pulse:
                            var rt = new RotateTransform();
                            rt.BeginAnimation(RotateTransform.AngleProperty, PulseAnimation);
                            b.RenderTransform = rt;
                            break;

                        default:
                            b.RenderTransform = null;
                            break;
                    }

                    b.Visibility = Visibility.Visible;
                }
                else
                {
                    b.Visibility = Visibility.Collapsed;
                }
            }
        }
    }
}
