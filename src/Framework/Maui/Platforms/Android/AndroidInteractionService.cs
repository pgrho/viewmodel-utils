using Android.Views;
using Android.Widget;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Shipwreck.BootstrapControls;

namespace Shipwreck.ViewModelUtils;

public class AndroidInteractionService : InteractionService
{
    #region Toasts

    public override bool SupportsToasts => true;

    public override Task ShowSuccessToastAsync(object context, string message, string title)
        => ShowToast(context, message, title, BorderStyle.Success, ColorScheme.Success);

    public override Task ShowErrorToastAsync(object context, string message, string title)
        => ShowToast(context, message, title, BorderStyle.Danger, ColorScheme.Danger);

    public override Task ShowWarningToastAsync(object context, string message, string title)
        => ShowToast(context, message, title, BorderStyle.Warning, ColorScheme.Warning);

    public override Task ShowInformationToastAsync(object context, string message, string title)
        => ShowToast(context, message, title, BorderStyle.Info, ColorScheme.Info);

    protected virtual Task ShowToast(object context, string message, string title, BorderStyle style, ColorScheme scheme)
    {
        if (context is IHasFrameworkPageViewModel hp
                && hp.Page is FrameworkPageViewModel pvm)
        {
            pvm.EnqueueToastLog(style, message, title);

            if (pvm.OverridesToast(message, title, style))
            {
                return Task.CompletedTask;
            }
        }

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
