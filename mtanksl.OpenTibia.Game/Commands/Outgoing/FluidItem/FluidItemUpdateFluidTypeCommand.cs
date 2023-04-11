using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class FluidItemUpdateFluidTypeCommand : Command
    {
        public FluidItemUpdateFluidTypeCommand(FluidItem fluidItem, FluidType fluidType)
        {
            FluidItem = fluidItem;

            FluidType = fluidType;
        }

        public FluidItem FluidItem { get; set; }

        public FluidType FluidType { get; set; }

        public override Promise Execute()
        {
            return Promise.Run( (resolve, reject) =>
            {
                if (FluidItem.FluidType != FluidType)
                {
                    FluidItem.FluidType = FluidType;

                    switch (FluidItem.Parent)
                    {
                        case Tile tile:

                            Context.AddCommand(new TileRefreshItemCommand(tile, FluidItem) );
                  
                            break;

                        case Inventory inventory:

                            Context.AddCommand(new InventoryRefreshItemCommand(inventory, FluidItem) );
                   
                            break;

                        case Container container:

                            Context.AddCommand(new ContainerRefreshItemCommand(container, FluidItem) );

                            break;
                    }
                }

                resolve();
            } );            
        }
    }
}