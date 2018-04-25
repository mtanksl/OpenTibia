namespace OpenTibia
{
    public class Teleport : Item
    {
        public Teleport(ItemMetadata metadata) : base(metadata)
        {

        }

        public Position Position { get; set; }
    }
}