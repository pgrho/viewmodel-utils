using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

#if NET5_0
using Notifications.Wpf.Core;
using Notifications.Wpf.Core.Controls;
#else

using Notifications.Wpf;
using Notifications.Wpf.Controls;

#endif

namespace Shipwreck.ViewModelUtils
{
    public class FrameworkInteractionService : InteractionService, IInteractionService
    {
        public static string NotificationAreaName { get; } = "NotificationArea";

        private NotificationManager _NotificationManager;

        protected NotificationManager NotificationManager
            => _NotificationManager ??= new NotificationManager();

        public TimeSpan? ToastExpirationTime { get; } = TimeSpan.FromSeconds(3);

        protected override void ShowToast(object context, string message, string title, BorderStyle style)
        {
            if (GetWindow(context)?.FindName(NotificationAreaName) is NotificationArea)
            {
                var content = new NotificationContent()
                {
                    Type = (style & BorderStyle.Success) != 0 ? NotificationType.Success
                        : (style & BorderStyle.Danger) != 0 ? NotificationType.Error
                        : (style & BorderStyle.Warning) != 0 ? NotificationType.Warning
                        : NotificationType.Information,
                    Title = title,
                    Message = message
                };

#if NET5_0
                NotificationManager.ShowAsync(content, expirationTime: ToastExpirationTime, areaName: NotificationAreaName).GetHashCode();
#else
                NotificationManager.Show(content, expirationTime: ToastExpirationTime, areaName: NotificationAreaName);
#endif

                return;
            }

            base.ShowToast(context, message, title, style);
        }

        protected override Task ShowModalAsync(object context, FrameworkElement frameworkElement)
        {
            if (GetWindow(context) is MetroWindow mw
                && DialogParticipation.GetRegister(mw) != null)
            {
                ConfigureViewModel(frameworkElement.DataContext);

                var tcs = new TaskCompletionSource<object>();

                if (frameworkElement is BaseMetroDialog d)
                {
                    d.Unloaded += (s, e) => tcs.TrySetResult(null);
                    mw.ShowMetroDialogAsync(d);
                }
                else
                {
                    var gd = new GenericMetroDialog()
                    {
                        DataContext = frameworkElement.DataContext,
                        Content = frameworkElement,
                    };
                    gd.Unloaded += (s, e) => tcs.TrySetResult(null);
                    mw.ShowMetroDialogAsync(gd);
                }
                return tcs.Task;
            }

            return base.ShowModalAsync(context, frameworkElement);
        }

        protected override bool CloseModal(object context, object viewModel)
        {
            var app = Application.Current;
            if (app != null)
            {
                foreach (var mw in app.Windows.OfType<MetroWindow>().ToList())
                {
                    if (DialogParticipation.GetRegister(mw) != null)
                    {
                        var dc = mw.FindChild<Panel>("PART_MetroActiveDialogContainer");
                        if (dc != null)
                        {
                            foreach (var d in dc.Children.OfType<BaseMetroDialog>().Where(e => e.DataContext == viewModel).ToList())
                            {
                                mw.HideMetroDialogAsync(d).GetHashCode();
                            }
                        }
                    }
                }
            }
            return base.CloseModal(context, viewModel);
        }
    }
}
