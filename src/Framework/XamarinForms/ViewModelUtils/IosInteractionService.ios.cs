namespace Shipwreck.ViewModelUtils;

public class IosInteractionService : InteractionService
{
    #region Toasts

    public override bool SupportsToasts => true;

    private const double SHORT_DELAY = 2.0;

    public override Task ShowSuccessToastAsync(object context, string message, string title)
        => ShowToast(context, message, title, BorderStyle.Success);

    public override Task ShowErrorToastAsync(object context, string message, string title)
        => ShowToast(context, message, title, BorderStyle.Danger);

    public override Task ShowWarningToastAsync(object context, string message, string title)
        => ShowToast(context, message, title, BorderStyle.Warning);

    public override Task ShowInformationToastAsync(object context, string message, string title)
        => ShowToast(context, message, title, BorderStyle.Info);

    protected virtual Task ShowToast(object context, string message, string title, BorderStyle style)
    {
        ShowAlert(context, message, title, style, SHORT_DELAY);
        return Task.CompletedTask;
    }

    protected void ShowAlert(object context, string message, string title, BorderStyle style, double seconds)
    {
        if (context is IHasFrameworkPageViewModel hp
                && hp.Page is FrameworkPageViewModel pvm)
        {
            pvm.EnqueueToastLog(style, message, title);

            if (pvm.OverridesToast(message, title, style))
            {
                return;
            }
        }

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
