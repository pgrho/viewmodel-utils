using Xamarin.Forms;

namespace Shipwreck.ViewModelUtils.Demo.XamarinForms
{
    public partial class MainPage : ContentPage
    {
        private class MainPageViewModel : ObservableModel
        {
        }

        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel();
        }
    }
}
