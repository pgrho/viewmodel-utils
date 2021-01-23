using Xamarin.Forms;

namespace Shipwreck.ViewModelUtils.Demo.XamarinForms
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            new FontAwesome5.FontAwesomeLabel { };
        }
    }

    public sealed class MainPageViewModel : ObservableModel
    {
        #region Icon

        private string _Icon = "fas fa-spinner fa-pulse";

        public string Icon
        {
            get => _Icon;
            set => SetProperty(ref _Icon, value);
        }

        #endregion Icon
    }
}
