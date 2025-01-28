namespace OpenTibia.Common.Objects
{
    public class StackableItem : Item
    {
        public StackableItem(ItemMetadata metadata) : base(metadata)
        {

        }

        public byte Count { get; set; }

        public override uint GetWeight()
        {
            return Count * base.GetWeight();
        }
    }
}