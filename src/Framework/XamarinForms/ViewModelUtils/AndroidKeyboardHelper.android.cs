using Shipwreck.ViewModelUtils;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidKeyboardHelper))]
namespace Shipwreck.ViewModelUtils;
using Application = Android.App.Application;

public class AndroidKeyboardHelper : IKeyboardHelper
{
    public void HideKeyboard()
    {
        if (Application.Context is Activity a
            && a?.GetSystemService(Context.InputMethodService) is InputMethodManager imm
            && a.CurrentFocus != null)
        {
            imm.HideSoftInputFromWindow(a.CurrentFocus.WindowToken, HideSoftInputFlags.None);

            a.Window.DecorView.ClearFocus();
        }
    }
}
