namespace OpenTibia.Common.Objects
{
    public class ReadableItem : Item
    {
        public ReadableItem(ItemMetadata metadata) : base(metadata)
        {

        }

        public string Text { get; set; }

        public string WrittenBy { get; set; }

        public uint WrittenDate { get; set; }
    }
}