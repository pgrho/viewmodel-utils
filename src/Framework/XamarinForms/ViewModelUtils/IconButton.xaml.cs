using TypeConverterAttribute = Xamarin.Forms.TypeConverterAttribute;

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
}
