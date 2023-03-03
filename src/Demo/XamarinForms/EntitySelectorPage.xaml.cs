using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Shipwreck.ViewModelUtils.Demo.XamarinForms;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class EntitySelectorPage : ContentPage
{
    public EntitySelectorPage()
    {
        InitializeComponent();
        BindingContext = new EntitySelectorPageViewModel(this);
    }
}
