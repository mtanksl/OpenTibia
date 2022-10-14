namespace OpenTibia.Common.Objects
{
    public class ReadableItem : Item
    {
        public ReadableItem(ItemMetadata metadata) : base(metadata)
        {

        }

        public string Text { get; set; }

        public string Author { get; set; }

        public string Date { get; set; }
    }
}