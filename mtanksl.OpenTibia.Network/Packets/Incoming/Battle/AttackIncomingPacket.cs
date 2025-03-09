using OpenTibia.Common.Objects;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class AttackIncomingPacket : IIncomingPacket
    {
        public uint CreatureId { get; set; }

        public uint Nonce { get; set; }
        
        public void Read(IByteArrayStreamReader reader, IHasFeatureFlag features)
        {
            CreatureId = reader.ReadUInt();

            if (features.HasFeatureFlag(Common.Structures.FeatureFlag.AttackSequence) )
            {
                Nonce = reader.ReadUInt();
            }
        }
    }
}