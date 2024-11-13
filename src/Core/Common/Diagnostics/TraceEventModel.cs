namespace Shipwreck.ViewModelUtils.Diagnostics;

public sealed class TraceEventModel
{
    internal TraceEventModel(
        TraceEventCache eventCache,
        string source,
        TraceEventType eventType,
        int id,
        string message,
        object[] data)
    {
        ProcessId = eventCache.ProcessId;
        ThreadId = eventCache.ThreadId;
        DateTime = eventCache.DateTime;

        Source = source;

        EventType = eventType;
        Id = id;

        Message = message != null ? (data?.Length > 0 ? string.Format(message, data) : message)
                : data != null ? string.Join(" ", data) : null;
    }

    public int ProcessId { get; }
    public string ThreadId { get; }
    public DateTime DateTime { get; }
    public string Source { get; }

    public TraceEventType EventType { get; }
    public int Id { get; }

    public string? Message { get; }
}
