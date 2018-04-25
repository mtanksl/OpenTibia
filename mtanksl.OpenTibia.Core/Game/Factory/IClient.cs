using System;

namespace OpenTibia
{
    public interface IClient : IDisposable
    {
        void Start();

        void Stop(bool wait = true);

        event EventHandler<DisconnectEventArgs> Disconnect;
    }
}