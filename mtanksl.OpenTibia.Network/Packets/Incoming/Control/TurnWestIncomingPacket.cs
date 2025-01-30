using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class TurnWestIncomingPacket : IIncomingPacket
    {
        public Direction Direction
        {
            get
            {
                return Direction.West;
            }
        }

        public void Read(IByteArrayStreamReader reader)
        {

        }
    }
}