namespace OpenTibia.Common.Objects
{
    public class StackableItem : Item
    {
        public StackableItem(ItemMetadata metadata) : base(metadata)
        {

        }

        public override uint Weight
        {
            get
            {
                return base.Weight * Count;
            }
        }

        public byte Count { get; set; }
    }
}