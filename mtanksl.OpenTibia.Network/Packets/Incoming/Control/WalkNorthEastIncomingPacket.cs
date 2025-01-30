using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class WalkNorthEastIncomingPacket : IIncomingPacket
    {
        public MoveDirection MoveDirection
        {
            get
            {
                return MoveDirection.NorthEast;
            }
        }

        public void Read(IByteArrayStreamReader reader)
        {

        }
    }
}