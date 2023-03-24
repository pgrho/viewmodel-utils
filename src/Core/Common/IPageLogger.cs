namespace Shipwreck.ViewModelUtils;

public interface IPageLogger
{
    void Log(TraceEventType eventType, int eventId, string message);
    void Log(TraceEventType eventType, int eventId, string format, object[] args);
}
