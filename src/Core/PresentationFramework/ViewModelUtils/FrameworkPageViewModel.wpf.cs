using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace Shipwreck.ViewModelUtils
{
    public partial class FrameworkPageViewModel
    {
        public struct ProcessingDisabled : IDisposable
        {
            private readonly Dispatcher _Dispatcher;
            private readonly DispatcherProcessingDisabled _Disabled;

            public ProcessingDisabled(Dispatcher dispatcher, DispatcherProcessingDisabled disabled)
            {
                _Dispatcher = dispatcher;
                _Disabled = disabled;
            }

            public void Dispose()
            {
                if (_Dispatcher != null)
                {
                    try
                    {
                        if (_Dispatcher.Thread == Thread.CurrentThread)
                        {
                            _Disabled.Dispose();
                        }
                        else
                        {
                            var ds = _Disabled;
                            _Dispatcher.Invoke((Action)(() => ds.Dispose()));
                        }
                    }
                    catch { }
                }
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
                        return new ProcessingDisabled(dp, dp.DisableProcessing());
                    }
                    else
                    {
                        DispatcherProcessingDisabled ds = default;
                        dp.Invoke((Action)(() => ds = dp.DisableProcessing()));

                        return new ProcessingDisabled(dp, ds);
                    }
                }
            }
            catch { }
            return default;
        }

        static partial void PlatformGetInteractionService(ref IInteractionService service)
            => service = new InteractionService();
    }
}
