﻿using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class SlotAddOutgoingPacket : IOutgoingPacket
    {
        public SlotAddOutgoingPacket(Slot slot, Item item)
        {
            this.Slot = slot;

            this.Item = item;
        }

        public Slot Slot { get; set; }

        public Item Item { get; set; }
        
        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x78 );

            writer.Write( (byte)Slot );

            writer.Write(Item);
        }
    }
}