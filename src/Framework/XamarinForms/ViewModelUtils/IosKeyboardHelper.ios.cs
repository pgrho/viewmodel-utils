using Shipwreck.ViewModelUtils;

[assembly: Xamarin.Forms.Dependency(typeof(IosKeyboardHelper))]
namespace Shipwreck.ViewModelUtils;
public class IosKeyboardHelper : IKeyboardHelper
{
    public void HideKeyboard()
    {
        UIApplication.SharedApplication.KeyWindow.EndEditing(true);
    }
}
