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
    }
}