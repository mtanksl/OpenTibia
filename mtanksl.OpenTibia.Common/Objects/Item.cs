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

        public bool LoadedFromMap { get; set; }

        public ushort ActionId { get; set; }

        public ushort UniqueId { get; set; }

        public IContainer Root()
        {
            IContainer container = Parent;

            while (container is IContent content)
            {
                container = content.Parent;
            }

            return container;
        }

        public bool IsContentOf(Container container)
        {
            IContent content = this;

            while (content != null)
            {
                if (content == container)
                {
                    return true;
                }

                content = content.Parent as IContent;
            }

            return false;
        }

        public virtual uint GetWeight()
        {
            return metadata.Weight ?? 0;
        }

        public override string ToString()
        {
            return "Id: " + Id + " OpenTibiaId: " + metadata.OpenTibiaId + " Name: " + metadata.Name;
        }
    }
}