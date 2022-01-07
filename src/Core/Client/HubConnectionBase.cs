using System;
using System.Threading.Tasks;
namespace Shipwreck.ViewModelUtils.Client
{
    public abstract class HubConnectionBase : IDisposable
    {
        private protected IFrameworkRealTimeConnection Connection { get; }

        internal HubConnectionBase(IFrameworkRealTimeConnection connection)
            => Connection = connection;

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
}
