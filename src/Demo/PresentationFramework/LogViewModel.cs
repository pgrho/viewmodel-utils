using System;
using System.Threading;

namespace Shipwreck.ViewModelUtils.Demo.PresentationFramework
{
    public sealed class LogViewModel
    {
        public LogViewModel(string message)
        {
            DateTime = DateTime.Now;
            ThreadId = Thread.CurrentThread.ManagedThreadId;
            Message = message;
        }

        public DateTime DateTime { get; }

        public int ThreadId { get; }

        public string Message { get; }
    }
}
