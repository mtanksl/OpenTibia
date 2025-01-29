using OpenTibia.Game.Common;
using OpenTibia.Network.Sockets;
using System.Net.Sockets;

namespace OpenTibia.Common
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
            server.Statistics.IncreaseActiveConnection();

            if ( !server.RateLimiting.IncreaseActiveConnection(IpAddress) )
            {
                Disconnect(DisconnectionType.RateLimited);

                return false;
            }

            return true;
        }

        protected override void DecreaseActiveConnection()
        {
            server.Statistics.DecreaseActiveConnection();

            server.RateLimiting.DecreaseActiveConnection(IpAddress);
        }

        protected override bool IsConnectionCountOk()
        {
            if ( !server.RateLimiting.IsConnectionCountOk(IpAddress) )
            {
                Disconnect(DisconnectionType.RateLimited);

                return false;
            }

            return true;
        }

        protected override bool IsPacketCountOk()
        {
            if ( !server.RateLimiting.IsPacketCountOk(IpAddress) )
            {
                Disconnect(DisconnectionType.RateLimited);

                return false;
            }

            return true;
        }

        protected override void IncreaseSlowSocket()
        {
            server.RateLimiting.IncreaseSlowSocket(IpAddress);

            Disconnect(DisconnectionType.SlowSocket);
        }

        protected override void IncreaseInvalidMessage()
        {
            if ( !server.RateLimiting.IncreaseInvalidMessage(IpAddress) )
            {
                Disconnect(DisconnectionType.RateLimited);
            }
        }

        protected override void IncreaseUnknownPacket()
        {
            if ( !server.RateLimiting.IncreaseUnknownPacket(IpAddress) )
            {
                Disconnect(DisconnectionType.RateLimited);
            }
        }

        protected override void OnReceived(byte[] body, int length)
        {
            server.Statistics.IncreaseMessagesReceived();

            server.Statistics.IncreaseBytesReceived(length);

            base.OnReceived(body, length);
        }

        protected override void OnSent(byte[] bytes, int length)
        {
            server.Statistics.IncreaseMessagesSent();

            server.Statistics.IncreaseBytesSent(length);

            base.OnSent(bytes, length);
        }
    }
}