namespace OpenTibia
{
    public class Stackable : Item
    {
        public Stackable(ItemMetadata metadata) : base(metadata)
        {

        }

        public byte Count { get; set; }
    }
}