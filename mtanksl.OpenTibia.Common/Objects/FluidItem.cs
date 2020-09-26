using OpenTibia.Common.Structures;

namespace OpenTibia.Common.Objects
{
    public class FluidItem : Item
    {
        public FluidItem(ItemMetadata metadata) : base(metadata)
        {

        }

        public FluidType FluidType { get; set; }
    }
}