using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Commands
{
    public class SplashItemUpdateFluidTypeCommand : Command
    {
        public SplashItemUpdateFluidTypeCommand(SplashItem splashItem, FluidType fluidType)
        {
            SplashItem = splashItem;

            FluidType = fluidType;
        }

        public SplashItem SplashItem { get; set; }

        public FluidType FluidType { get; set; }

        public override Promise Execute()
        {
            if (SplashItem.FluidType != FluidType)
            {
                SplashItem.FluidType = FluidType;

                switch (SplashItem.Parent)
                {
                    case Tile tile:

                        return Context.AddCommand(new TileRefreshItemCommand(tile, SplashItem) );

                    case Inventory inventory:

                        return Context.AddCommand(new InventoryRefreshItemCommand(inventory, SplashItem) );

                    case Container container:

                        return Context.AddCommand(new ContainerRefreshItemCommand(container, SplashItem) );
                }
            }

            return Promise.Completed;
        }
    }
}