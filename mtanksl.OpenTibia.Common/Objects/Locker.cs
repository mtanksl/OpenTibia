namespace OpenTibia.Common.Objects
{
    public class Locker : Container
    {
        public Locker(ItemMetadata metadata) : base(metadata)
        {

        }

        public Town Town { get; set; }
    }
}