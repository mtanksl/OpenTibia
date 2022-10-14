using OpenTibia.Common.Structures;

namespace OpenTibia.Common.Objects
{
    public class SplashItem : Item
    {
        public SplashItem(ItemMetadata metadata) : base(metadata)
        {

        }

        public FluidType FluidType { get; set; }
    }
}