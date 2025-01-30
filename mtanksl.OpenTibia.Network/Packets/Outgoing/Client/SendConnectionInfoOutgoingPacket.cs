using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class SendConnectionInfoOutgoingPacket : IOutgoingPacket
    {
        public SendConnectionInfoOutgoingPacket(uint nonce)
        {
            this.Nonce = nonce;
        }

        public uint Nonce { get; set; }

        public void Write(IByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x1F );

            writer.Write(Nonce);

            writer.Write( (byte)0x00 );
        }
    }
}