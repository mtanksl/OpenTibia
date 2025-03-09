using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class SetPartyIconOutgoingPacket : IOutgoingPacket
    {
        public SetPartyIconOutgoingPacket(uint creatureId, PartyIcon partyIcon)
        {
            this.CreatureId = creatureId;

            this.PartyIcon = partyIcon;
        }

        public uint CreatureId { get; set; }

        public PartyIcon PartyIcon { get; set; }
        
        public void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features)
        {
            writer.Write( (byte)0x91 );

            writer.Write(CreatureId);

            writer.Write( (byte)PartyIcon );
        }
    }
}