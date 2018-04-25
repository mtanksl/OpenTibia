using OpenTibia.IO;

namespace OpenTibia
{
    public class FrameOutgoingPacket : IOutgoingPacket
    {
        public FrameOutgoingPacket(uint creatureId, FrameColor frameColor)
        {
            this.CreatureId = creatureId;

            this.FrameColor = frameColor;
        }

        public uint CreatureId { get; set; }

        public FrameColor FrameColor { get; set; }
        
        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x86 );

            writer.Write(CreatureId);

            writer.Write( (byte)FrameColor );
        }
    }
}