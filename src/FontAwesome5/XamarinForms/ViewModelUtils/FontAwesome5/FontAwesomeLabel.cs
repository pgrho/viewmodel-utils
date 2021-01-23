using System;
using Xamarin.Forms;

namespace Shipwreck.ViewModelUtils.FontAwesome5
{
    public sealed class FontAwesomeLabel : Label
    {
        public static readonly BindableProperty IsPressedProperty
            = BindableProperty.Create(nameof(IsPressed), typeof(bool), typeof(FontAwesomeLabel), defaultValue: false);

        public bool IsPressed
        {
            get => (bool)GetValue(IsPressedProperty);
            set => SetValue(IsPressedProperty, value);
        }

        public static readonly BindableProperty IconProperty
            = BindableProperty.Create(nameof(Icon), typeof(string), typeof(FontAwesomeLabel),
                null,
                propertyChanged: (b, o, v) =>
                {
                    if (b is FontAwesomeLabel l)
                    {
                        if (v is string s && FontAwesome.TryParse(s, out var family, out var text, out var animation))
                        {
                            l.IsVisible = true;
                            l.Animation = animation;
                            l.FontFamily = family;
                            l.Text = text;
                            return;
                        }
                        l.IsVisible = false;
                    }
                });

        public string Icon
        {
            get => (string)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public static readonly BindableProperty AnimationProperty
            = BindableProperty.Create(nameof(Animation), typeof(FontAwesomeAnimation), typeof(FontAwesomeLabel), FontAwesomeAnimation.None,
                propertyChanged: (b, o, v) => (b as FontAwesomeLabel)?.OnAnimationChanged((FontAwesomeAnimation)o, (FontAwesomeAnimation)v));

        public FontAwesomeAnimation Animation
        {
            get => (FontAwesomeAnimation)GetValue(AnimationProperty);
            set => SetValue(AnimationProperty, value);
        }

        private void OnAnimationChanged(FontAwesomeAnimation oldValue, FontAwesomeAnimation newValue)
        {
            Rotation = 0;
            ViewExtensions.CancelAnimations(this);

            switch (newValue)
            {
                case FontAwesomeAnimation.Spin:
                    this.RotateTo(3600 * 24 * 180, 3600 * 24, Easing.Linear);
                    break;

                case FontAwesomeAnimation.Pulse:
                    ViewExtensions.CancelAnimations(this);
                    Device.StartTimer(TimeSpan.FromSeconds(0.125), () =>
                    {
                        if (Animation == FontAwesomeAnimation.Pulse)
                        {
                            Rotation += 45;
                            return true;
                        }
                        return false;
                    });
                    break;
            }
        }
    }
}
