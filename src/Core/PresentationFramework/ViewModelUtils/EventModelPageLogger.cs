namespace Shipwreck.ViewModelUtils;

public class EventModelPageLogger : IPageLogger
{
    private static readonly int ProcessId = Process.GetCurrentProcess()?.Id ?? 0;

    public EventModelPageLogger(string name)
    {
        Name = name;
    }

    public string Name { get; }

    public void Log(TraceEventType eventType, int eventId, string message)
        => EventModel.Add(
            new EventModel(
                ProcessId,
                Thread.CurrentThread?.ManagedThreadId.ToString(),
                DateTime.Now,
                Name,
                eventType,
                eventId,
                message));

    public void Log(TraceEventType eventType, int eventId, string format, object[] args)
        => EventModel.Add(
            new EventModel(
                ProcessId,
                Thread.CurrentThread?.ManagedThreadId.ToString(),
                DateTime.Now,
                Name,
                eventType,
                eventId,
                string.Format(format, args)));
}
