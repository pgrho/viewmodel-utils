namespace Shipwreck.ViewModelUtils.Demo.Maui;

public partial class InteractionPage : ContentPage
{
	public InteractionPage()
	{
		InitializeComponent();
        BindingContext = new InteractionPageViewModel(this);
    }
}
