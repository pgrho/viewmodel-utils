namespace Shipwreck.ViewModelUtils.Demo.Maui;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
    }
}
