using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class SlotRemove : IOutgoingPacket
    {
        public SlotRemove(Slot slot)
        {
            this.Slot = slot;
        }

        public Slot Slot { get; set; }
        
        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x79 );

            writer.Write( (byte)Slot );
        }
    }
}