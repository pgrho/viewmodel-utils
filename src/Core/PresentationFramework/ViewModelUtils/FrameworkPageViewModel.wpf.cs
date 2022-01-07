using System.Windows.Threading;

namespace Shipwreck.ViewModelUtils;

public partial class FrameworkPageViewModel
{
    public struct ProcessingDisabled : IDisposable
    {
        private readonly DispatcherProcessingDisabled _Disabled;

        public ProcessingDisabled(DispatcherProcessingDisabled disabled)
        {
            _Disabled = disabled;
        }

        public void Dispose()
        {
            try
            {
                _Disabled.Dispose();
            }
            catch { }
        }
    }

    public ProcessingDisabled DisableProcessing()
    {
        try
        {
            var dp = Application.Current?.Dispatcher;
            if (dp != null)
            {
                if (dp.Thread == Thread.CurrentThread)
                {
                    return new ProcessingDisabled(dp.DisableProcessing());
                }
            }
        }
        catch { }
        return default;
    }

    static partial void PlatformGetInteractionService(ref IInteractionService service)
        => service = new InteractionService();
}
