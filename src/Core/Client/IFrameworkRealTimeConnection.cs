namespace Shipwreck.ViewModelUtils.Client;

public interface IFrameworkRealTimeConnection : IDisposable
{
    void On(string hubName, string eventName, Action onData);

    void On<T>(string hubName, string eventName, Action<T> onData);

    void On<T1, T2>(string hubName, string eventName, Action<T1, T2> onData);

    void On<T1, T2, T3>(string hubName, string eventName, Action<T1, T2, T3> onData);

    void On<T1, T2, T3, T4>(string hubName, string eventName, Action<T1, T2, T3, T4> onData);

    void On<T1, T2, T3, T4, T5>(string hubName, string eventName, Action<T1, T2, T3, T4, T5> onData);

    void On<T1, T2, T3, T4, T5, T6>(string hubName, string eventName, Action<T1, T2, T3, T4, T5, T6> onData);

    void On<T1, T2, T3, T4, T5, T6, T7>(string hubName, string eventName, Action<T1, T2, T3, T4, T5, T6, T7> onData);

    Task StartAsync();

    Task Invoke(string hubName, string methodName, params object[] args);

    Task<T> Invoke<T>(string hubName, string methodName, params object[] args);

    Task StopAsync();
}
