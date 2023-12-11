using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class WalkWestIncomingPacket : IIncomingPacket
    {
        public MoveDirection MoveDirection
        {
            get
            {
                return MoveDirection.West;
            }
        }

        public void Read(ByteArrayStreamReader reader)
        {

        }
    }
}