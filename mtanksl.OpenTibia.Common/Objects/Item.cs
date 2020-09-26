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

        public IContainer Container { get; set; }

        public IContainer GetRootContainer()
        {
            IContainer container = Container;

            while (container is IContent content)
            {
                container = content.Container;
            }

            return container;
        }        

        public bool IsChild(Item parent)
        {
            Item item = this;

            while (item != null)
            {
                if (item == parent)
                {
                    return true;
                }

                item = item.Container as Item;
            }

            return false;
        }
    }
}