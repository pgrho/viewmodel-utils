using System.Windows;

#if NET5_0
using Notifications.Wpf.Core.Controls;
#else

using Notifications.Wpf.Controls;

#endif

namespace Shipwreck.ViewModelUtils.Controls
{
    public class FrameworkNotificationArea : NotificationArea
    {
        static FrameworkNotificationArea()
        {
#if NET5_0
            BindableNameProperty.OverrideMetadata(
                typeof(FrameworkNotificationArea),
                new FrameworkPropertyMetadata(FrameworkInteractionService.NotificationAreaName));
#endif
        }
    }
}
