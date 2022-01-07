namespace Shipwreck.ViewModelUtils.Client;

public abstract class HubConnectionBase : IDisposable
{
    protected HubConnectionBase(IFrameworkRealTimeConnection connection)
        => Connection = connection;

    protected IFrameworkRealTimeConnection Connection { get; }

    public Task StartAsync() => Connection.StartAsync();

    public Task StopAsync() => Connection.StopAsync();

    public async void Dispose()
    {
        try
        {
            await Connection.StopAsync().ConfigureAwait(false);
        }
        catch { }
        try
        {
            Connection.Dispose();
        }
        catch { }
    }
}
