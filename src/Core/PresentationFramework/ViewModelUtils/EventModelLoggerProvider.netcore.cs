#if NET7_0_OR_GREATER
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Shipwreck.ViewModelUtils;

[ProviderAlias("EventModel")]
public sealed class EventModelLoggerProvider : ILoggerProvider
{
    private readonly IDisposable _onChangeToken;
    private EventModelLoggerConfiguration _currentConfig;
    private readonly ConcurrentDictionary<string, EventModelLogger> _loggers;

    public EventModelLoggerProvider(IOptionsMonitor<EventModelLoggerConfiguration> config)
    {
        _loggers = new(StringComparer.OrdinalIgnoreCase);
        _currentConfig = config.CurrentValue;
        _onChangeToken = config.OnChange(updatedConfig => _currentConfig = updatedConfig);
    }

    public ILogger CreateLogger(string categoryName)
        => _loggers.GetOrAdd(categoryName, name => new EventModelLogger(name, GetCurrentConfig));

    private EventModelLoggerConfiguration GetCurrentConfig() => _currentConfig;

    public void Dispose()
    {
        _loggers.Clear();
        _onChangeToken?.Dispose();
    }
}
#endif
