using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Converters;
using System.ComponentModel;

namespace Shipwreck.ViewModelUtils;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class IconButton : ContentView
{
    public static readonly BindableProperty FontSizeProperty
        = BindableProperty.Create(
            nameof(FontSize), typeof(double), typeof(IconButton),
            defaultValue: new FontSizeConverter().ConvertFromInvariantString("Large"));

    public static readonly BindableProperty ButtonPaddingProperty
        = BindableProperty.Create(
            nameof(ButtonPadding), typeof(Thickness), typeof(IconButton),
            defaultValue: new Thickness(16, 0));

    public static readonly BindableProperty HasBorderProperty
        = BindableProperty.Create(nameof(HasBorder), typeof(bool), typeof(IconButton), defaultValue: true);

    public IconButton()
    {
        InitializeComponent();
        Padding = new Thickness(0);

        OnButtonPaddingChanged();
        OnFontSizeChanged();
        OnHasBorderChanged();

        OnFrameTextColorChanged();

        frame.PropertyChanged += (s, e) =>
        {
            switch (e.PropertyName)
            {
                case nameof(frame.TextColor):
                    OnFrameTextColorChanged();
                    break;
            }
        };
    }

    [TypeConverter(typeof(FontSizeConverter))]
    public double FontSize
    {
        get => (double)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    [TypeConverter(typeof(ThicknessTypeConverter))]
    public Thickness ButtonPadding
    {
        get => (Thickness)GetValue(ButtonPaddingProperty);
        set => SetValue(ButtonPaddingProperty, value);
    }

    public bool HasBorder
    {
        get => (bool)GetValue(HasBorderProperty);
        set => SetValue(HasBorderProperty, value);
    }

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        switch (propertyName)
        {
            case nameof(ButtonPadding):
                OnButtonPaddingChanged();
                break;

            case nameof(FontSize):
                OnFontSizeChanged();
                break;

            case nameof(HasBorder):
                OnHasBorderChanged();
                break;
        }
    }

    private void OnButtonPaddingChanged()
    {
        if (frame != null)
        {
            frame.Padding = ButtonPadding;
        }
    }

    private void OnFontSizeChanged()
    {
        if (icon != null)
        {
            icon.FontSize = FontSize;
        }
    }

    private void OnHasBorderChanged()
    {
        if (frame != null)
        {
            frame.HasBorder = HasBorder;
        }
    }

    private void OnFrameTextColorChanged()
    {
        if (icon != null && frame != null)
        {
            icon.TextColor = frame.TextColor;
        }
    }
}
