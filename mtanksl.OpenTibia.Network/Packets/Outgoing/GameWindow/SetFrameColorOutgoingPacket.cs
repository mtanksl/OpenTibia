using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class SetFrameColorOutgoingPacket : IOutgoingPacket
    {
        public SetFrameColorOutgoingPacket(uint creatureId, FrameColor frameColor)
        {
            this.CreatureId = creatureId;

            this.FrameColor = frameColor;
        }

        public uint CreatureId { get; set; }

        public FrameColor FrameColor { get; set; }
        
        public void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features)
        {
            if ( !features.HasFeatureFlag(FeatureFlag.PVPFrame) ) 
            {
                writer.Write( (byte)0x86);

                writer.Write(CreatureId);

                writer.Write( (byte)FrameColor );
            }
            else //TODO: Which version was this implemented?
            {
                writer.Write( (byte)0x93);

                writer.Write(CreatureId);

                writer.Write( (byte)0x01); // 0x00 = Permanent, 0x01 = Timed

                writer.Write( (byte)FrameColor );
            }
        }
    }
}