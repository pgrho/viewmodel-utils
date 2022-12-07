#if NET5_0_OR_GREATER
using Notifications.Wpf.Core.Controls;
#else

using Notifications.Wpf.Controls;

#endif

namespace Shipwreck.ViewModelUtils.Controls;

public class FrameworkNotificationArea : NotificationArea
{
    static FrameworkNotificationArea()
    {
#if NET5_0_OR_GREATER
        BindableNameProperty.OverrideMetadata(
            typeof(FrameworkNotificationArea),
            new FrameworkPropertyMetadata(FrameworkInteractionService.NotificationAreaName));
#endif
    }
}
