using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class TurnSouthIncomingPacketh : IIncomingPacket
    {
        public Direction Direction
        {
            get
            {
                return Direction.South;
            }
        }

        public void Read(ByteArrayStreamReader reader)
        {

        }
    }
}