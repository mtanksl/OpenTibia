using OpenTibia.Common.Objects;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class StopAttackAndFollowOutgoingPacket : IOutgoingPacket
    {
        public StopAttackAndFollowOutgoingPacket(uint nonce)
        {
            this.Nonce = nonce;
        }

        public uint Nonce { get; set; }

        public void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features)
        {
            writer.Write( (byte)0xA3 );

            if (features.HasFeatureFlag(Common.Structures.FeatureFlag.AttackSequence) )
            {
                writer.Write(Nonce);
            }
        }
    }
}