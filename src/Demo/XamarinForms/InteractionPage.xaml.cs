using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Shipwreck.ViewModelUtils.Demo.XamarinForms;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class InteractionPage : ContentPage
{
    public InteractionPage()
    {
        InitializeComponent();
        BindingContext = new InteractionPageViewModel(this);
    }
}
