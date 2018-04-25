using OpenTibia.IO;

namespace OpenTibia
{
    public class ThingRemoveOutgoingPacket : IOutgoingPacket
    {
        public ThingRemoveOutgoingPacket(Position position, byte index)
        {
            this.Position = position;

            this.Index = index;
        }

        public Position Position { get; set; }

        public byte Index { get; set; }
        
        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x6C );

            writer.Write(Position.X);

            writer.Write(Position.Y);

            writer.Write(Position.Z);

            writer.Write(Index);
        }
    }
}