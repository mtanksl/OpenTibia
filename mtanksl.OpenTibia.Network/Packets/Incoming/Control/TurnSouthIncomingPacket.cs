using OpenTibia.Common.Objects;
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

        public void Read(IByteArrayStreamReader reader, IHasFeatureFlag features)
        {

        }
    }
}