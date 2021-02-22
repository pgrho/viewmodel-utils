using System;

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
    }
}
