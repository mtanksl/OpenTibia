using OpenTibia.Game;
using OpenTibia.Network.Sockets;
using System.Net.Sockets;

namespace OpenTibia.Common.Objects
{
    public abstract class RateLimitingConnection : Connection
    {
        private IServer server;

        public RateLimitingConnection(IServer server, Socket socket) : base(socket, server.Config.SocketReceiveTimeoutMilliseconds, server.Config.SocketSendTimeoutMilliseconds)
        {
            this.server = server;
        }

        protected override bool IncreaseActiveConnection()
        {
            if ( !server.RateLimiting.IncreaseActiveConnection(IpAddress) )
            {
                OnDisconnected(new DisconnectedEventArgs(DisconnectionType.RateLimited) );

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

        protected override void IncreaseInvalidMessage()
        {
            if ( !server.RateLimiting.IncreaseInvalidMessage(IpAddress) )
            {
                OnDisconnected(new DisconnectedEventArgs(DisconnectionType.RateLimited) );
            }
        }

        protected override void IncreaseUnknownPacket()
        {
            if ( !server.RateLimiting.IncreaseUnknownPacket(IpAddress) )
            {
                OnDisconnected(new DisconnectedEventArgs(DisconnectionType.RateLimited) );
            }
        }
    }
}