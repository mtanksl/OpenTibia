using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.Extensions
{
    public static class FluidItemExtensions
    {
        /// <exception cref="InvalidOperationException"></exception>

        public static Promise UpdateFluidType(this FluidItem item, FluidType fluidType)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new FluidItemUpdateFluidTypeCommand(item, fluidType) );
        }
    }
}