using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class WalkSouthEastIncomingPacket : IIncomingPacket
    {
        public MoveDirection MoveDirection
        {
            get
            {
                return MoveDirection.SouthEast;
            }
        }

        public void Read(IByteArrayStreamReader reader)
        {

        }
    }
}