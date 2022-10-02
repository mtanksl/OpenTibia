using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Extensions
{
    public static class FluidItemExtensions
    {
        public static Promise UpdateFluidType(this FluidItem item, FluidType fluidType)
        {
            Context context = Context.Current;

            return context.AddCommand(new FluidItemUpdateFluidTypeCommand(item, fluidType) );
        }
    }
}