using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class Walk : IOutgoingPacket
    {
        public Walk(Position fromPosition, byte fromIndex, Position toPosition)
        {
            this.FromPosition = fromPosition;

            this.FromIndex = fromIndex;

            this.ToPosition = toPosition;
        }

        public Position FromPosition { get; set; }

        public byte FromIndex { get; set; }

        public Position ToPosition { get; set; }
        
        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x6D );

            writer.Write(FromPosition.X);

            writer.Write(FromPosition.Y);

            writer.Write(FromPosition.Z);

            writer.Write(FromIndex);

            writer.Write(ToPosition.X);

            writer.Write(ToPosition.Y);

            writer.Write(ToPosition.Z);
        }
    }
}