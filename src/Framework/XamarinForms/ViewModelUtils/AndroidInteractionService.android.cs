using System.Threading.Tasks;
using Android.Views;
using Android.Widget;
using Shipwreck.BootstrapControls;
using Xamarin.Forms.Platform.Android;

namespace Shipwreck.ViewModelUtils
{
    public class AndroidInteractionService : InteractionService
    {
        #region Toasts

        public override bool SupportsToasts => true;

        public override Task ShowSuccessToastAsync(object context, string message, string title)
            => ShowToast(message, ColorScheme.Success);

        public override Task ShowErrorToastAsync(object context, string message, string title)
            => ShowToast(message, ColorScheme.Danger);

        public override Task ShowWarningToastAsync(object context, string message, string title)
            => ShowToast(message, ColorScheme.Warning);

        public override Task ShowInformationToastAsync(object context, string message, string title)
            => ShowToast(message, ColorScheme.Info);

        protected virtual Task ShowToast(string message, ColorScheme scheme)
        {
            var t = Toast.MakeText(Android.App.Application.Context, message, ToastLength.Short);
            t.SetGravity(GravityFlags.Top | GravityFlags.Center, 0, 30);
            t.View?.SetBackgroundColor(scheme.BackgroundColor.ToAndroid());
            if (t.View?.FindViewById<TextView>(Android.Resource.Id.Message) is TextView tv)
            {
                tv.SetTextColor(scheme.ForegroundColor.ToAndroid());
            }
            t.Show();
            return Task.CompletedTask;
        }

        #endregion Toasts
    }
}
