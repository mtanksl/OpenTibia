namespace OpenTibia.Common.Objects
{
    public class Locker : Container
    {
        public Locker(ItemMetadata metadata) : base(metadata)
        {

        }

        public ushort TownId { get; set; }
    }
}