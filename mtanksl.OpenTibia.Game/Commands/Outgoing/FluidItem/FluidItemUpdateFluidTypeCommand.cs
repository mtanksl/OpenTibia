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

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                if (FluidItem.FluidType != FluidType)
                {
                    FluidItem.FluidType = FluidType;

                    switch (FluidItem.Parent)
                    {
                        case Tile tile:

                            context.AddCommand(new TileRefreshItemCommand(tile, FluidItem) );
                  
                            break;

                        case Inventory inventory:

                            context.AddCommand(new InventoryRefreshItemCommand(inventory, FluidItem) );
                   
                            break;

                        case Container container:

                            context.AddCommand(new ContainerRefreshItemCommand(container, FluidItem) );

                            break;
                    }
                }

                resolve(context);
            } );            
        }
    }
}