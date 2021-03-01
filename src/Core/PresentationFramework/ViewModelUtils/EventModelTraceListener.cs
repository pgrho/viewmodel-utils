using System;
using System.Diagnostics;

namespace Shipwreck.ViewModelUtils
{
    public class EventModelTraceListener : TraceListener
    {
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
            => EventModel.Add(new EventModel(eventCache, source, eventType, id, null, new[] { data }));

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
            => EventModel.Add(new EventModel(eventCache, source, eventType, id, null, data));

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id)
            => EventModel.Add(new EventModel(eventCache, source, eventType, id, null, null));

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
            => EventModel.Add(new EventModel(eventCache, source, eventType, id, message, null));

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
            => EventModel.Add(new EventModel(eventCache, source, eventType, id, format, args));

        public override void TraceTransfer(TraceEventCache eventCache, string source, int id, string message, Guid relatedActivityId)
            => EventModel.Add(new EventModel(eventCache, source, TraceEventType.Transfer, id, message, new object[] { relatedActivityId }));

        public override void Write(string message)
            => WriteLine(message);

        public override void WriteLine(string message)
            => EventModel.Add(new EventModel(new TraceEventCache(), string.Empty, TraceEventType.Verbose, 0, message, null));
    }
}
