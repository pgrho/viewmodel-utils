using Android.Content;
using Android.Views.InputMethods;

namespace Shipwreck.ViewModelUtils;

public class AndroidKeyboardService : IKeyboardService
{
    public void Hide()
    {
        if (Android.App.Application.Context is Android.App.Activity a
            && a?.GetSystemService(Context.InputMethodService) is InputMethodManager imm
            && a.CurrentFocus != null)
        {
            imm.HideSoftInputFromWindow(a.CurrentFocus.WindowToken, HideSoftInputFlags.None);

            a.Window.DecorView.ClearFocus();
        }
    }
}
