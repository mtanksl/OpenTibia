namespace OpenTibia.Common.Objects
{
    public class ItemMetadata
    {
        public ushort OpenTibiaId { get; set; }

        public ushort TibiaId { get; set; }

        public TopOrder TopOrder { get; set; }

        public ushort Speed { get; set; }

        public bool IsContainer { get; set; }

        public byte Capacity { get; set; }

        public bool Stackable { get; set; }

        public string Name { get; set; }
    }
}