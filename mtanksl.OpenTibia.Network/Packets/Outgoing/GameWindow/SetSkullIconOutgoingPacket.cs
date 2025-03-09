using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class SetSkullIconOutgoingPacket : IOutgoingPacket
    {
        public SetSkullIconOutgoingPacket(uint creatureId, SkullIcon skullIcon)
        {
            this.CreatureId = creatureId;

            this.SkullIcon = skullIcon;
        }

        public uint CreatureId { get; set; }

        public SkullIcon SkullIcon { get; set; }
        
        public void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features)
        {
            writer.Write( (byte)0x90 );

            writer.Write(CreatureId);

            writer.Write( (byte)SkullIcon );
        }
    }
}