using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Events
{
    public class ContainerReplaceItemEventArgs : GameEventArgs
    {
        public ContainerReplaceItemEventArgs(Container container, Item fromItem, Item toItem, byte index)
        {
            Container = container;

            FromItem = fromItem;

            ToItem = toItem;

            Index = index;
        }

        public Container Container { get; set; }

        public Item FromItem { get; set; }

        public Item ToItem { get; set; }

        public byte Index { get; set; }
    }
}