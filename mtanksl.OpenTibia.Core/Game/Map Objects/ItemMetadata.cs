namespace OpenTibia
{
    public class ItemMetadata
    {
        //otb
        public ushort ClientId { get; set; }

        public ushort ServerId { get; set; }

        //dat
        public TopOrder TopOrder { get; set; }

        public ushort Speed { get; set; }

        public ItemMetadataFlags Flags { get; set; }

        public Light Light { get; set; }
               
        //xml
        public string Article { get; set; }

        public string Name { get; set; }

        public string Plural { get; set; }

        public uint Weight { get; set; }

        public FloorChange FloorChange { get; set; }
    }
}