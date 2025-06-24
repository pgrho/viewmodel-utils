using System;
using System.Collections.Generic;
using System.Text;

namespace Shipwreck.ViewModelUtils;

public static class TaskSchedulerHelper
{
    public static bool FromCurrentSynchronizationContext { get; set; }
#if IS_WEBVIEW
    = true;
#endif

    public static TaskScheduler Default()
        => FromCurrentSynchronizationContext ? TaskScheduler.FromCurrentSynchronizationContext() : TaskScheduler.Current;
}
