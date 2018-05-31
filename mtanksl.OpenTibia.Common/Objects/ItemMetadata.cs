namespace OpenTibia.Common.Objects
{
    public class ItemMetadata
    {
        public ushort ServerId { get; set; }

        public ushort ClientId { get; set; }

        public TopOrder TopOrder { get; set; }

        public ushort Speed { get; set; }
    }
}