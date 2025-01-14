using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Events
{
    public class ContainerAddItemEventArgs : GameEventArgs
    {
        public ContainerAddItemEventArgs(Container container, Item item, byte index)
        {
            Container = container;

            Item = item;

            Index = index;
        }

        public Container Container { get; }

        public Item Item { get; }

        public byte Index { get; }
    }
}