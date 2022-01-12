using Application = Android.App.Application;

[assembly: Xamarin.Forms.Dependency(typeof(Shipwreck.ViewModelUtils.AndroidKeyboardService))]
namespace Shipwreck.ViewModelUtils;

public class AndroidKeyboardService : IKeyboardService
#pragma warning disable CS0612 // 型またはメンバーが旧型式です
    , IKeyboardHelper
#pragma warning restore CS0612 // 型またはメンバーが旧型式です
{
    public void Hide()
    {
        if (Application.Context is Activity a
            && a?.GetSystemService(Context.InputMethodService) is InputMethodManager imm
            && a.CurrentFocus != null)
        {
            imm.HideSoftInputFromWindow(a.CurrentFocus.WindowToken, HideSoftInputFlags.None);

            a.Window.DecorView.ClearFocus();
        }
    }
#pragma warning disable CS0612 // 型またはメンバーが旧型式です
    void IKeyboardHelper.HideKeyboard() => Hide();
#pragma warning restore CS0612 // 型またはメンバーが旧型式です
}
