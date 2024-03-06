using Android.App;
using Android.Runtime;

namespace Shipwreck.ViewModelUtils.Demo.Maui;

[Application]
public class MainApplication : MauiApplication
{
    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
        : base(handle, ownership)
    {
    }

    protected override MauiApp CreateMauiApp()
    {
        DependencyService.Register<AndroidInteractionService>();
        DependencyService.Register<AndroidKeyboardService>();

        return MauiProgram.CreateMauiApp();
    }
}
