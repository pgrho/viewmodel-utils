using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Shipwreck.BootstrapControls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using System.Threading.Tasks;

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

        private Task ShowToast(string message, ColorScheme scheme)
        {
            var t = Toast.MakeText(Android.App.Application.Context, message, ToastLength.Short);
            t.SetGravity(GravityFlags.Top | GravityFlags.Center, 0, 30);
            t.View.SetBackgroundColor(scheme.BackgroundColor.ToAndroid());
            var tv = t.View.FindViewById<TextView>(Android.Resource.Id.Message);
            if (tv != null)
            {
                tv.SetTextColor(scheme.ForegroundColor.ToAndroid());
            }
            t.Show();
            return Task.CompletedTask;
        }

        #endregion Toasts
    }
}
