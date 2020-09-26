using OpenTibia.Common.Objects;

namespace OpenTibia.Common.Events
{
    public class ContainerAddItemEventArgs : GameEventArgs
    {
        public ContainerAddItemEventArgs(Container container, Item item, byte index)
        {
            Container = container;

            Item = item;

            Index = index;
        }

        public Container Container { get; set; }

        public Item Item { get; set; }

        public byte Index { get; set; }
    }
}