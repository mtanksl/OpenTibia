using OpenTibia.Common.Objects;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class SlotAddOutgoingPacket : IOutgoingPacket
    {
        public SlotAddOutgoingPacket(byte slot, Item item)
        {
            this.Slot = slot;

            this.Item = item;
        }

        public byte Slot { get; set; }

        public Item Item { get; set; }
        
        public void Write(IByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x78 );

            writer.Write(Slot);

            writer.Write(Item);
        }
    }
}