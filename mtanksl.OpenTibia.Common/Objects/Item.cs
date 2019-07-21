using OpenTibia.Common.Structures;

namespace OpenTibia.Common.Objects
{
    public class Item : IContent
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

        public Item GetParent()
        {
            Item item = this;

            while (true)
            {
                Item parent = item.Container as Item;

                if (parent == null)
                {
                    break;
                }

                item = parent;
            }

            return item;
        }

        public bool IsChildOfParent(Item parent)
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