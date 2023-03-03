using Xamarin.Forms;

namespace Shipwreck.ViewModelUtils.Demo.XamarinForms;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        BindingContext = new MainPageViewModel(this);
    }
}
