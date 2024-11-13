#if NET9_0_OR_GREATER
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Shipwreck.ViewModelUtils;
[ProviderAlias("EventModel")]
public sealed class EventModelLogger : ILogger
{
    private static readonly int ProcessId = Process.GetCurrentProcess()?.Id ?? 0;
    private readonly string _Name;

    internal EventModelLogger(string name, Func<EventModelLoggerConfiguration> config)
    {
        _Name = name;
    }

    public IDisposable BeginScope<TState>(TState state) where TState : notnull
        => null;

    public bool IsEnabled(LogLevel logLevel)
        => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        var em = new EventModel(
            ProcessId,
            Thread.CurrentThread?.ManagedThreadId.ToString(),
            DateTime.Now,
            _Name,
            logLevel switch
            {
                LogLevel.Critical => TraceEventType.Critical,
                LogLevel.Error => TraceEventType.Error,
                LogLevel.Warning => TraceEventType.Warning,
                LogLevel.Information => TraceEventType.Information,
                _ => TraceEventType.Verbose,
            }, eventId.Id, formatter?.Invoke(state, exception));

        EventModel.Add(em);
    }
}
#endif
