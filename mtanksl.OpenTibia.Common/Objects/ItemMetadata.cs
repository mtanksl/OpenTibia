using OpenTibia.Common.Structures;

namespace OpenTibia.Common.Objects
{
    public class ItemMetadata
    {
        public ushort OpenTibiaId { get; set; }

        public ushort TibiaId { get; set; }

        public TopOrder TopOrder { get; set; }

        public ItemMetadataFlags Flags { get; set; }

        public ushort Speed { get; set; }

        public Light Light { get; set; }

        public string Name { get; set; }

        public byte Capacity { get; set; }
    }
}