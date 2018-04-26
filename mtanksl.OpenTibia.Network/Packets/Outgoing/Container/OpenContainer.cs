using OpenTibia.IO;
using System.Collections.Generic;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class OpenContainer : IOutgoingPacket
    {
        public OpenContainer(byte containerId, ushort itemId, string name, byte capacity, bool hasParent, List<Item> items)
        {
            this.ContainerId = containerId;

            this.ItemId = itemId;

            this.Name = name;

            this.Capacity = capacity;

            this.HasParent = hasParent;

            this.items = items;
        }

        public byte ContainerId { get; set; }

        public ushort ItemId { get; set; }

        public string Name { get; set; }

        public byte Capacity { get; set; }

        public bool HasParent { get; set; }

        private List<Item> items;

        public List<Item> Items
        {
            get
            {
                return items ?? ( items = new List<Item>() );
            }
            set
            {
                items = value;
            }
        }
        
        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x6E );

            writer.Write(ContainerId);

            writer.Write(ItemId);

            writer.Write(Name);

            writer.Write(Capacity);

            writer.Write(HasParent);

            writer.Write( (byte)Items.Count );

            foreach (var item in Items)
            {
                writer.Write(item);
            }
        }
    }
}