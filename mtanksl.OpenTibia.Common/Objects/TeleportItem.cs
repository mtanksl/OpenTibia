using OpenTibia.Common.Structures;

namespace OpenTibia.Common.Objects
{
    public class TeleportItem : Item
    {
        public TeleportItem(ItemMetadata metadata) : base(metadata)
        {

        }

        public Position Position { get; set; }
    }
}