using Android.Graphics.Drawables;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Controls;
using static Android.Icu.Text.CaseMap;

namespace Shipwreck.ViewModelUtils;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class CommandButton : ContentView
{
    public static readonly BindableProperty FontSizeProperty
        = BindableProperty.Create(
            nameof(FontSize), typeof(double), typeof(CommandButton),
            defaultValue: new FontSizeConverter().ConvertFromInvariantString("Default"));

    public static readonly BindableProperty ButtonPaddingProperty
        = BindableProperty.Create(nameof(ButtonPadding), typeof(Thickness), typeof(CommandButton), defaultValue: new Thickness(8));

    public CommandButton()
    {
        InitializeComponent();

        OnButtonPaddingChanged();
        OnFontSizeChanged();
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

    public Thickness ButtonPadding
    {
        get => (Thickness)GetValue(ButtonPaddingProperty);
        set => SetValue(ButtonPaddingProperty, value);
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

        if (title != null)
        {
            title.FontSize = FontSize;
        }

        if (badgeCount != null)
        {
            badgeCount.FontSize = FontSize;
        }
    }

    private void OnFrameTextColorChanged()
    {
        if (frame != null)
        {
            if (icon != null)
            {
                icon.TextColor = frame.TextColor;
            }

            title.TextColor = frame.TextColor;
        }
    }
}
