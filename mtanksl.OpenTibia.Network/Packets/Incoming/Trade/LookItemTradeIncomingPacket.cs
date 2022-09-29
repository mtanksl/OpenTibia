using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class LookItemTradeIncomingPacket : IIncomingPacket
    {
        public byte WindowId { get; set; }

        public byte Index { get; set; }
        
        public void Read(ByteArrayStreamReader reader)
        {
            WindowId = reader.ReadByte();

            Index = reader.ReadByte();
        }
    }
}