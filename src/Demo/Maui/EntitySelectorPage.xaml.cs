namespace Shipwreck.ViewModelUtils.Demo.Maui;

public partial class EntitySelectorPage : ContentPage
{
	public EntitySelectorPage()
	{
		InitializeComponent();
        BindingContext = new EntitySelectorPageViewModel(this);
    }
}
