[assembly: Xamarin.Forms.Dependency(typeof(Shipwreck.ViewModelUtils.IosKeyboardService))]

namespace Shipwreck.ViewModelUtils;

public class IosKeyboardService : IKeyboardService
#pragma warning disable CS0612 // 型またはメンバーが旧型式です
    , IKeyboardHelper
#pragma warning restore CS0612 // 型またはメンバーが旧型式です
{
    public void Hide()
        => UIApplication.SharedApplication.KeyWindow.EndEditing(true);

#pragma warning disable CS0612 // 型またはメンバーが旧型式です
    void IKeyboardHelper.HideKeyboard() => Hide();
#pragma warning restore CS0612 // 型またはメンバーが旧型式です
}
