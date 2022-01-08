namespace Shipwreck.ViewModelUtils;

public static class ActivityHelper
{
    public static void HandleKeyDown(this Activity activity, Keycode keyCode)
    {
        for (var mp = Xamarin.Forms.Application.Current?.MainPage; mp != null;)
        {
            if (mp is IKeyDownHandler kdh)
            {
                string s;
                if (Keycode.A <= keyCode && keyCode <= Keycode.Z)
                {
                    s = new string((char)(keyCode - Keycode.A + 'a'), 1);
                }
                else if (Keycode.Num0 <= keyCode && keyCode <= Keycode.Num9)
                {
                    s = (keyCode - Keycode.Num0).ToString("D");
                }
                else if (Keycode.Numpad0 <= keyCode && keyCode <= Keycode.Numpad9)
                {
                    s = (keyCode - Keycode.Numpad0).ToString("D");
                }
                else
                {
                    s = keyCode switch
                    {
                        Keycode.Enter => System.Environment.NewLine,
                        Keycode.Plus => "+",
                        Keycode.Minus => "-",
                        Keycode.NumpadSubtract => "-",
                        Keycode.NumpadMultiply => "*",
                        Keycode.Slash => "/",
                        Keycode.NumpadDivide => "/",
                        Keycode.Equals => "=",
                        Keycode.NumpadEquals => "=",
                        Keycode.Comma => ",",
                        Keycode.Period => ".",
                        Keycode.NumpadDot => ".",
                        Keycode.NumpadLeftParen => "(",
                        Keycode.NumpadRightParen => ")",
                        _ => null,
                    };
                }

                if (s != null)
                {
                    kdh.OnKeyDown(s);
                }

                break;
            }
            if (mp is FlyoutPage fp)
            {
                mp = fp.IsPresented ? fp.Flyout : fp.Detail;
            }
            else if (mp is NavigationPage np)
            {
                mp = np.CurrentPage;
            }
            else
            {
                break;
            }
        }
    }
}
