using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Shipwreck.ViewModelUtils
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CommandButton : ContentView
    {
        public static readonly BindableProperty FontSizeProperty
            = BindableProperty.Create(
                nameof(FontSize), typeof(double), typeof(CommandButton),
                defaultValue: new FontSizeConverter().ConvertFromInvariantString("Default"));

        public CommandButton()
        {
            InitializeComponent();
            Padding = new Thickness(8);
        }

        [TypeConverter(typeof(FontSizeConverter))]
        public double FontSize
        {
            get => (double)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }
    }
}
