using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.IO;
using System.Collections.Generic;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class OpenContainerOutgoingPacket : IOutgoingPacket
    {
        public OpenContainerOutgoingPacket(byte containerId, Container container, string name, byte capacity, bool hasParent, bool isUnlocked, bool hasPages, ushort firstIndex, List<Item> items)
        {
            this.ContainerId = containerId;

            this.Container = container;

            this.Name = name;

            this.Capacity = capacity;

            this.HasParent = hasParent;

            this.IsUnlocked = isUnlocked;

            this.HasPages = hasPages;

            this.FirstIndex = firstIndex;

            this.items = items;
        }

        public byte ContainerId { get; set; }

        public Container Container { get; set; }

        public string Name { get; set; }

        public byte Capacity { get; set; }

        public bool HasParent { get; set; }

        public bool IsUnlocked { get; set; }

        public bool HasPages { get; set; }

        public ushort FirstIndex { get; set; }

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

            writer.Write(features, Container);

            writer.Write(Name);

            writer.Write(Capacity);

            writer.Write(HasParent);

            if (features.HasFeatureFlag(FeatureFlag.ContainerPagination) )
            {
                writer.Write(IsUnlocked);

                writer.Write(HasPages);

                writer.Write( (ushort)items.Count);

                writer.Write(FirstIndex);
            }

            writer.Write( (byte)Items.Count );

            foreach (var item in Items)
            {
                writer.Write(features, item);
            }
        }
    }
}