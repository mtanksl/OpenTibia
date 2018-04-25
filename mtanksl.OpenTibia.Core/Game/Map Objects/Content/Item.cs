namespace OpenTibia
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

        public ushort ActionId { get; set; }

        public ushort UniqueId { get; set; }

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