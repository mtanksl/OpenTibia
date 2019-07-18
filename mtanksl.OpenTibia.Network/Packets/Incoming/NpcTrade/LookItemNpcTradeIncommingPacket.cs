using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class LookItemNpcTradeIncommingPacket : IIncomingPacket
    {
        public ushort ItemId { get; set; }

        public byte Type { get; set; }
        
        public void Read(ByteArrayStreamReader reader)
        {
            ItemId = reader.ReadUShort();

            Type = reader.ReadByte();
        }
    }
}