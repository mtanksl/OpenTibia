using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class SlotRemoveOutgoingPacket : IOutgoingPacket
    {
        public SlotRemoveOutgoingPacket(byte slot)
        {
            this.Slot = slot;
        }

        public byte Slot { get; set; }
        
        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x79 );

            writer.Write(Slot);
        }
    }
}