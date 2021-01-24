using Xamarin.Forms;

namespace Shipwreck.ViewModelUtils.Demo.XamarinForms
{
    public partial class MainPage : ContentPage
    {
        class MainPageViewModel : ObservableModel
        {
            #region Icon

            private string _Icon = "fas fa-spinner fa-pulse";

            public string Icon
            {
                get => _Icon;
                set => SetProperty(ref _Icon, value);
            }

            #endregion Icon

            #region IsPressed

            private bool _IsPressed;
            public bool IsPressed
            {
                get => _IsPressed;
                set => SetProperty(ref _IsPressed, value);
            }

            #endregion IsPressed
        }

        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel();
        }
    }

}
