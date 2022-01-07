namespace Shipwreck.ViewModelUtils;

public class ConsolePageLogger : IPageLogger
{
    public ConsolePageLogger(string name)
    {
        Name = name;
    }

    public string Name { get; }

    public void Log(TraceEventType eventType, int eventId, string message)
        => Console.WriteLine("[{0}][{1}][{2}]{3}", Name, eventType, eventId, message);

    public void Log(TraceEventType eventType, int eventId, string format, object[] args)
        => Console.WriteLine("[{0}][{1}][{2}]{3}", Name, eventType, eventId, string.Format(format, args));
}
