namespace Shipwreck.ViewModelUtils;

public class IosKeyboardService : IKeyboardService
{
    public void Hide()
    {
        UIApplication.SharedApplication.KeyWindow.EndEditing(true);
    }
}
