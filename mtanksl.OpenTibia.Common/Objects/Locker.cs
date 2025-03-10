namespace OpenTibia.Common.Objects
{
    public class Locker : Container
    {
        public Locker(ItemMetadata metadata) : base(metadata)
        {

        }

        public Locker(ItemMetadata metadata, int internalListCapacity) : base(metadata, internalListCapacity)
        {

        }

        public ushort TownId { get; set; }
    }
}