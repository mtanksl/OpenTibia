using OpenTibia.Game.Common;
using System.Net.Sockets;

namespace OpenTibia.Common
{
    public abstract class RawConnection : RateLimitingConnection
    {
        public RawConnection(IServer server, Socket socket) : base(server, socket)
        {

        }

        protected override byte[] Envelope(byte[] bytes)
        {
            return bytes;
        }
    }
}