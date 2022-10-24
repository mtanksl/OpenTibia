using OpenTibia.Common.Structures;

namespace OpenTibia.Common.Objects
{
    public class Item : GameObject, IContent
    {
        public Item(ItemMetadata metadata)
        {
            this.metadata = metadata;
        }

        private ItemMetadata metadata;

        public ItemMetadata Metadata
        {
            get
            {
                return metadata;
            }
        }

        public TopOrder TopOrder
        {
            get
            {
                return metadata.TopOrder;
            }
        }

        public IContainer Parent { get; set; }

        /*
        public Container Container
        {
            get
            {
                return Parent as Container;
            }
        }

        public Inventory Inventory
        {
            get
            {
                return Parent as Inventory;
            }
        }

        public Tile Tile
        {
            get
            {
                return Parent as Tile;
            }
        }
        */

        public IContainer Root()
        {
            IContainer container = Parent;

            while (container is IContent content)
            {
                container = content.Parent;
            }

            return container;
        }

        public bool IsContainerOf(Item child)
        {
            return child.IsContentOf(this);
        }

        public bool IsContentOf(Item parent)
        {
            IContent item = this;

            while (item != null)
            {
                if (item == parent)
                {
                    return true;
                }

                item = item.Parent as IContent;
            }

            return false;
        }
    }
}