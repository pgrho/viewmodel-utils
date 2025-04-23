namespace Shipwreck.ViewModelUtils.Diagnostics;

public sealed class ObservableCollectionTraceListener : TraceListener
{
    public static ObservableCollection<TraceEventModel> Data { get; }
        = new ObservableCollection<TraceEventModel>();

    public static object SyncRoot { get; } = new object();

    private static int _MaximumCount = 5000;

    public int MaximumCount
    {
        get => _MaximumCount;
        set => _MaximumCount = value;
    }

    private static void Add(TraceEventModel item)
    {
        lock (SyncRoot)
        {
            while (Data.Count >= _MaximumCount)
            {
                Data.RemoveAt(Data.Count - _MaximumCount);
            }
            Data.Add(item);
        }
    }

    public override void TraceData(TraceEventCache? eventCache, string source, TraceEventType eventType, int id, object? data)
        => Add(new TraceEventModel(eventCache, source, eventType, id, null, new[] { data }));

    public override void TraceData(TraceEventCache? eventCache, string source, TraceEventType eventType, int id, params object?[]? data)
        => Add(new TraceEventModel(eventCache, source, eventType, id, null, data));

    public override void TraceEvent(TraceEventCache? eventCache, string source, TraceEventType eventType, int id)
        => Add(new TraceEventModel(eventCache, source, eventType, id, null, null));

    public override void TraceEvent(TraceEventCache? eventCache, string source, TraceEventType eventType, int id, string? message)
        => Add(new TraceEventModel(eventCache, source, eventType, id, message, null));

    public override void TraceEvent(TraceEventCache? eventCache, string source, TraceEventType eventType, int id, string? format, params object?[]? args)
        => Add(new TraceEventModel(eventCache, source, eventType, id, format, args));

    public override void TraceTransfer(TraceEventCache? eventCache, string source, int id, string? message, Guid relatedActivityId)
        => Add(new TraceEventModel(eventCache, source, TraceEventType.Transfer, id, message, new object[] { relatedActivityId }));

    public override void Write(string? message)
        => throw new NotSupportedException();

    public override void WriteLine(string? message)
        => throw new NotSupportedException();
}
