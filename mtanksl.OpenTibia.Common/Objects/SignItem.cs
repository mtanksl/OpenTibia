namespace OpenTibia.Common.Objects
{
    public class SignItem : Item
    {
        public SignItem(ItemMetadata metadata) : base(metadata)
        {

        }

        public string Text { get; set; }
    }
}