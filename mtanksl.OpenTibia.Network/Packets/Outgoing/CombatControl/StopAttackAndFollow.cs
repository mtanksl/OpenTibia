using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class StopAttackAndFollow : IOutgoingPacket
    {
        public StopAttackAndFollow(uint nonce)
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