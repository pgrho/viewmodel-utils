using System;
using System.Windows;
using System.Windows.Threading;

namespace Shipwreck.ViewModelUtils
{
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
                => _Disabled.Dispose();
        }

        public ProcessingDisabled DisableProcessing()
            => new ProcessingDisabled(Application.Current?.Dispatcher.DisableProcessing() ?? default);

        static partial void PlatformGetInteractionService(ref IInteractionService service)
            => service = new InteractionService();
    }
}
