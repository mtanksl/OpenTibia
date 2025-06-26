using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Events
{
    public class ContainerReplaceItemEventArgs : GameEventArgs
    {
        public ContainerReplaceItemEventArgs(Container container, Item fromItem, Item toItem, ushort index)
        {
            Container = container;

            FromItem = fromItem;

            ToItem = toItem;

            Index = index;
        }

        public Container Container { get; }

        public Item FromItem { get; }

        public Item ToItem { get; }

        public ushort Index { get; }
    }
}