using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class SendSpellGroupCooldownOutgoingPacket : IOutgoingPacket
    {
        public SendSpellGroupCooldownOutgoingPacket(SpellGroup spellGroup, uint time)
        {
            this.SpellGroup = spellGroup;     
            
            this.Time = time;
        }

        public SpellGroup SpellGroup { get; set; }

        public uint Time { get; set; }
                
        public void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features)
        {
            writer.Write( (byte)0xA5);

            writer.Write( (byte)SpellGroup);

            writer.Write(Time);
        }
    }
}