using System.Threading.Tasks;
using Foundation;
using UIKit;

namespace Shipwreck.ViewModelUtils
{
    public class IosInteractionService : InteractionService
    {
        #region Toasts

        public override bool SupportsToasts => true;

        private const double SHORT_DELAY = 2.0;

        public override Task ShowSuccessToastAsync(object context, string message, string title)
            => ShowToast(message, title);

        public override Task ShowErrorToastAsync(object context, string message, string title)
        {
            return ShowToast(message, title);
        }

        public override Task ShowWarningToastAsync(object context, string message, string title)
        {
            return ShowToast(message, title);
        }

        public override Task ShowInformationToastAsync(object context, string message, string title)
            => ShowToast(message, title);

        protected virtual Task ShowToast(string message, string title)
        {
            ShowAlert(message, title, SHORT_DELAY);
            return Task.CompletedTask;
        }

        protected void ShowAlert(string message, string title, double seconds)
        {
            UIAlertController alert = null;
            var alertDelay = NSTimer.CreateScheduledTimer(seconds, obj =>
            {
                try
                {
                    alert?.DismissViewController(true, null);
                    obj?.Dispose();
                }
                catch { }
            });
            alert = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);
            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(alert, true, null);
        }

        #endregion Toasts
    }
}
