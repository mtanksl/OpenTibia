using OpenTibia.Common.Objects;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class SendSpellCooldownOutgoingPacket : IOutgoingPacket
    {
        public SendSpellCooldownOutgoingPacket(byte spellId, uint time)
        {
            this.SpellId = spellId;     
            
            this.Time = time;
        }

        public byte SpellId { get; set; }

        public uint Time { get; set; }
                
        public void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features)
        {
            writer.Write( (byte)0xA4);

            writer.Write(SpellId);

            writer.Write(Time);
        }
    }
}