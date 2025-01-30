using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class WalkNorthWestIncomingPacket : IIncomingPacket
    {
        public MoveDirection MoveDirection
        {
            get
            {
                return MoveDirection.NorthWest;
            }
        }

        public void Read(IByteArrayStreamReader reader)
        {

        }
    }
}