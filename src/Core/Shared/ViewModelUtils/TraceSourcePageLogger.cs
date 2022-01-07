namespace Shipwreck.ViewModelUtils;

public class TraceSourcePageLogger : IPageLogger
{
    public TraceSourcePageLogger(string name)
        : this(new TraceSource(name)) { }

    public TraceSourcePageLogger(TraceSource traceSouce)
    {
        TraceSource = traceSouce;
    }

    public TraceSource TraceSource { get; }

    public string Name => TraceSource.Name;

    public void Log(TraceEventType eventType, int eventId, string message)
        => TraceSource.TraceEvent(eventType, eventId, message);

    public void Log(TraceEventType eventType, int eventId, string format, object[] args)
        => TraceSource.TraceEvent(eventType, eventId, format, args);
}
