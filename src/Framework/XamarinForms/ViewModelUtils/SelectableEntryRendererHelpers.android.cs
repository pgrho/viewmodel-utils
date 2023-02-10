using Android.OS;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using AView = Android.Views.View;
using PlatformConfiguration = Xamarin.Forms.PlatformConfiguration;
using TextAlignment = Xamarin.Forms.TextAlignment;
using ATextAlignment = Android.Views.TextAlignment;

namespace Shipwreck.ViewModelUtils;

internal static class SelectableEntryRendererHelpers
{

    internal static void UpdateTextAlignment(this EditText view, TextAlignment horizontal, TextAlignment vertical)
    {
        if ((int)Build.VERSION.SdkInt < 17 || !view.Context.HasRtlSupport())
        {
            view.Gravity = vertical.ToVerticalGravityFlags() | horizontal.ToHorizontalGravityFlags();
        }
        else
        {
            view.TextAlignment = horizontal.ToTextAlignment();
            view.Gravity = vertical.ToVerticalGravityFlags();
        }
    }
    internal static ATextAlignment ToTextAlignment(this TextAlignment alignment)
    {
        switch (alignment)
        {
            case TextAlignment.Center:
                return ATextAlignment.Center;
            case TextAlignment.End:
                return ATextAlignment.ViewEnd;
            default:
                return ATextAlignment.ViewStart;
        }
    }
    internal static GravityFlags ToVerticalGravityFlags(this TextAlignment alignment)
    {
        switch (alignment)
        {
            case TextAlignment.Start:
                return GravityFlags.Top;
            case TextAlignment.End:
                return GravityFlags.Bottom;
            default:
                return GravityFlags.CenterVertical;
        }
    }
    internal static GravityFlags ToHorizontalGravityFlags(this TextAlignment alignment)
    {
        switch (alignment)
        {
            case TextAlignment.Center:
                return GravityFlags.CenterHorizontal;
            case TextAlignment.End:
                return GravityFlags.End;
            default:
                return GravityFlags.Start;
        }
    }
    public static ImeAction ToAndroidImeOptions(this PlatformConfiguration.AndroidSpecific.ImeFlags flags)
    {
        switch (flags)
        {
            case PlatformConfiguration.AndroidSpecific.ImeFlags.Previous:
                return ImeAction.Previous;
            case PlatformConfiguration.AndroidSpecific.ImeFlags.Next:
                return ImeAction.Next;
            case PlatformConfiguration.AndroidSpecific.ImeFlags.Search:
                return ImeAction.Search;
            case PlatformConfiguration.AndroidSpecific.ImeFlags.Send:
                return ImeAction.Send;
            case PlatformConfiguration.AndroidSpecific.ImeFlags.Go:
                return ImeAction.Go;
            case PlatformConfiguration.AndroidSpecific.ImeFlags.None:
                return ImeAction.None;
            case PlatformConfiguration.AndroidSpecific.ImeFlags.ImeMaskAction:
                return ImeAction.ImeMaskAction;
            case PlatformConfiguration.AndroidSpecific.ImeFlags.NoPersonalizedLearning:
                return (ImeAction)Android.Views.InputMethods.ImeFlags.NoPersonalizedLearning;
            case PlatformConfiguration.AndroidSpecific.ImeFlags.NoExtractUi:
                return (ImeAction)Android.Views.InputMethods.ImeFlags.NoExtractUi;
            case PlatformConfiguration.AndroidSpecific.ImeFlags.NoAccessoryAction:
                return (ImeAction)Android.Views.InputMethods.ImeFlags.NoAccessoryAction;
            case PlatformConfiguration.AndroidSpecific.ImeFlags.NoFullscreen:
                return (ImeAction)Android.Views.InputMethods.ImeFlags.NoFullscreen;
            case PlatformConfiguration.AndroidSpecific.ImeFlags.Default:
            case PlatformConfiguration.AndroidSpecific.ImeFlags.Done:
            default:
                return ImeAction.Done;
        }
    }
    internal static ImeAction ToAndroidImeAction(this ReturnType returnType)
    {
        switch (returnType)
        {
            case ReturnType.Go:
                return ImeAction.Go;
            case ReturnType.Next:
                return ImeAction.Next;
            case ReturnType.Send:
                return ImeAction.Send;
            case ReturnType.Search:
                return ImeAction.Search;
            case ReturnType.Done:
                return ImeAction.Done;
            case ReturnType.Default:
                return ImeAction.Done;
            default:
                throw new System.NotImplementedException($"ReturnType {returnType} not supported");
        }
    }
    internal static float ToEm(this double pt)
    {
        return (float)pt * 0.0624f; //Coefficient for converting Pt to Em
    }
    public static bool IsDisposed(this Java.Lang.Object obj)
    {
        return obj.Handle == IntPtr.Zero;
    }

    internal static bool UseLegacyColorManagement<T>(this T element) where T : Xamarin.Forms.VisualElement, IElementConfiguration<T>
    {
        // Determine whether we're letting the VSM handle the colors or doing it the old way
        // or disabling the legacy color management and doing it the old-old (pre 2.0) way
        return !element.HasVisualStateGroups()
                && element.OnThisPlatform().GetIsLegacyColorModeEnabled();
    }
    internal static void HideKeyboard(this AView inputView, bool overrideValidation = false)
    {
        if (inputView == null)
            throw new ArgumentNullException(nameof(inputView) + " must be set before the keyboard can be hidden.");

        using (var inputMethodManager = (InputMethodManager)inputView.Context.GetSystemService(Context.InputMethodService))
        {
            if (!overrideValidation && !(inputView is EditText || inputView is TextView || inputView is SearchView))
                throw new ArgumentException("inputView should be of type EditText, SearchView, or TextView");

            IBinder windowToken = inputView.WindowToken;
            if (windowToken != null && inputMethodManager != null)
                inputMethodManager.HideSoftInputFromWindow(windowToken, HideSoftInputFlags.None);
        }
    }

    internal static void ShowKeyboard(this TextView inputView)
    {
        if (inputView == null)
            throw new ArgumentNullException(nameof(inputView) + " must be set before the keyboard can be shown.");

        using (var inputMethodManager = (InputMethodManager)inputView.Context.GetSystemService(Context.InputMethodService))
        {
            // The zero value for the second parameter comes from 
            // https://developer.android.com/reference/android/view/inputmethod/InputMethodManager#showSoftInput(android.view.View,%20int)
            // Apparently there's no named value for zero in this case
            inputMethodManager?.ShowSoftInput(inputView, 0);
        }
    }
    internal static void ShowKeyboard(this AView view)
    {
        switch (view)
        {
            case SearchView searchView:
                searchView.ShowKeyboard();
                break;
            case TextView textView:
                textView.ShowKeyboard();
                break;
        }
    }

    internal static void PostShowKeyboard(this AView view)
    {
        void ShowKeyboard()
        {
            // Since we're posting this on the queue, it's possible something else will have disposed of the view
            // by the time the looper is running this, so we have to verify that the view is still usable
            if (view.IsDisposed())
            {
                return;
            }

            view.ShowKeyboard();
        };

        Device.BeginInvokeOnMainThread(ShowKeyboard);
    }
}
