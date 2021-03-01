using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.ViewModelUtils
{
    public interface IPageLogger
    {
        void Log(TraceEventType eventType, int eventId, string message);
        void Log(TraceEventType eventType, int eventId, string format, object[] args);
    }
}
