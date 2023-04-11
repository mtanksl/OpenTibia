using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

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
            return Promise.Run( (resolve, reject) =>
            {
                if (SplashItem.FluidType != FluidType)
                {
                    SplashItem.FluidType = FluidType;

                    switch (SplashItem.Parent)
                    {
                        case Tile tile:

                            Context.AddCommand(new TileRefreshItemCommand(tile, SplashItem) );
                  
                            break;

                        case Inventory inventory:

                            Context.AddCommand(new InventoryRefreshItemCommand(inventory, SplashItem) );
                   
                            break;

                        case Container container:

                            Context.AddCommand(new ContainerRefreshItemCommand(container, SplashItem) );

                            break;
                    }
                }

                resolve();
            } );            
        }
    }
}