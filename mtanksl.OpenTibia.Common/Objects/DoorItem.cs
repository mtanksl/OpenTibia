namespace OpenTibia.Common.Objects
{
    public class DoorItem : Item
    {
        public DoorItem(ItemMetadata metadata) : base(metadata)
        {

        }

        public byte DoorId { get; set; }
    }
}