using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.IO;
using System.Collections.Generic;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class SendBasicDataOutgoingPacket : IOutgoingPacket
    {
        public SendBasicDataOutgoingPacket(bool premium, Vocation vocation, List<int> spellIds)
        {
            this.Premium = premium;

            this.Vocation = vocation;

            this.SpellIds = spellIds;
        }

        public bool Premium { get; set; }

        public Vocation Vocation { get; set; }

        public List<int> SpellIds { get; set; }

        public void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features)
        {
            writer.Write( (byte)0x9F);

            writer.Write(Premium);

            writer.Write( (byte)Vocation);

            writer.Write( (ushort)SpellIds.Count);

            foreach (var spellId in SpellIds)
            {
                writer.Write( (byte)spellId);
            }
        }
    }
}