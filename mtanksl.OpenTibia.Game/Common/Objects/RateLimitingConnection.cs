using OpenTibia.Game;
using OpenTibia.Network.Sockets;
using System.Net.Sockets;

namespace OpenTibia.Common.Objects
{
    public abstract class RateLimitingConnection : Connection
    {
        private Server server;

        public RateLimitingConnection(Server server, Socket socket) : base(socket, server.Config.SocketReceiveTimeoutMilliseconds, server.Config.SocketSendTimeoutMilliseconds)
        {
            this.server = server;
        }

        protected override bool IncreaseActiveConnection()
        {
            if ( !server.RateLimiting.IncreaseActiveConnection(IpAddress) )
            {
                OnDisconnected(new DisconnectedEventArgs(DisconnectionType.MultiClient) );

                return false;
            }

            return true;
        }

        protected override void DecreaseActiveConnection()
        {
            server.RateLimiting.DecreaseActiveConnection(IpAddress);
        }

        protected override bool IsConnectionCountOk()
        {
            if ( !server.RateLimiting.IsConnectionCountOk(IpAddress) )
            {
                OnDisconnected(new DisconnectedEventArgs(DisconnectionType.RateLimited) );

                return false;
            }

            return true;
        }

        protected override bool IsPacketCountOk()
        {
            if ( !server.RateLimiting.IsPacketCountOk(IpAddress) )
            {
                OnDisconnected(new DisconnectedEventArgs(DisconnectionType.RateLimited) );

                return false;
            }

            return true;
        }

        protected override void IncreaseSlowSocket()
        {
            server.RateLimiting.IncreaseSlowSocket(IpAddress);

            OnDisconnected(new DisconnectedEventArgs(DisconnectionType.SlowSocket) );
        }
    }
}