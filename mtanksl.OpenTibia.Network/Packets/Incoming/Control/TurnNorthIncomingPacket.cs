using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class TurnNorthIncomingPacket : IIncomingPacket
    {
        public Direction Direction
        {
            get
            {
                return Direction.North;
            }
        }

        public void Read(IByteArrayStreamReader reader, IHasFeatureFlag features)
        {

        }
    }
}