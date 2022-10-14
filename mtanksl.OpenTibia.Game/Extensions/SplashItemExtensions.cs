using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Extensions
{
    public static class SplashItemExtensions
    {
        public static Promise UpdateFluidType(this SplashItem item, FluidType fluidType)
        {
            Context context = Context.Current;

            return context.AddCommand(new SplashItemUpdateFluidTypeCommand(item, fluidType) );
        }
    }
}