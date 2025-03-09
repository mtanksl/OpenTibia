using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class WalkSouthWestIncomingPacket : IIncomingPacket
    {
        public MoveDirection MoveDirection
        {
            get
            {
                return MoveDirection.SouthWest;
            }
        }

        public void Read(IByteArrayStreamReader reader, IHasFeatureFlag features)
        {

        }
    }
}