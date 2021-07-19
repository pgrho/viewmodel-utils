using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Shipwreck.BootstrapControls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
namespace Shipwreck.ViewModelUtils
{
    public class AndroidKeyboardService : IKeyboardService
    {
        public void Hide()
        {
            var context = Forms.Context;
            var inputMethodManager = context.GetSystemService(Context.InputMethodService) as InputMethodManager;
            if (inputMethodManager != null && context is Activity)
            {
                var activity = context as Activity;
                var token = activity.CurrentFocus?.WindowToken;
                inputMethodManager.HideSoftInputFromWindow(token, HideSoftInputFlags.None);

                activity.Window.DecorView.ClearFocus();
            }
        }
    }
}
