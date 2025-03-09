using OpenTibia.Common.Objects;
using OpenTibia.IO;
using System.Collections.Generic;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class OpenContainerOutgoingPacket : IOutgoingPacket
    {
        public OpenContainerOutgoingPacket(byte containerId, ushort tibiaId, string name, byte capacity, bool hasParent, List<Item> items)
        {
            this.ContainerId = containerId;

            this.TibiaId = tibiaId;

            this.Name = name;

            this.Capacity = capacity;

            this.HasParent = hasParent;

            this.items = items;
        }

        public byte ContainerId { get; set; }

        public ushort TibiaId { get; set; }

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
        
        public void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features)
        {
            writer.Write( (byte)0x6E );

            writer.Write(ContainerId);

            writer.Write(TibiaId);

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