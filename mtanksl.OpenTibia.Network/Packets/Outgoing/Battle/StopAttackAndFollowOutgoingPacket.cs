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

        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0xA3 );

            writer.Write(Nonce);
        }
    }
}