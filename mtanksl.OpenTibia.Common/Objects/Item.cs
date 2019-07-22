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

        public IContainer GetParent()
        {
            IContainer container = Container;

            while (container is IContent content)
            {
                container = content.Container;
            }

            return container;
        }

        public bool IsChildOfParent(IContent parent)
        {
            IContent item = this;

            while (item != null)
            {
                if (item == parent)
                {
                    return true;
                }

                item = item.Container as IContent;
            }

            return false;
        }
    }
}