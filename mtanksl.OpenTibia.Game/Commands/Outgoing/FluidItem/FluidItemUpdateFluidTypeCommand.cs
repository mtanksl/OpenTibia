using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class FluidItemUpdateFluidTypeCommand : Command
    {
        public FluidItemUpdateFluidTypeCommand(FluidItem item, FluidType fluidType)
        {
            Item = item;

            FluidType = fluidType;
        }

        public FluidItem Item { get; set; }

        public FluidType FluidType { get; set; }

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                if (Item.FluidType != FluidType)
                {
                    Item.FluidType = FluidType;

                    switch (Item.Parent)
                    {
                        case Tile tile:

                            context.AddCommand(new TileRefreshItemCommand(tile, Item) );
                  
                            break;

                        case Inventory inventory:

                            context.AddCommand(new InventoryRefreshItemCommand(inventory, Item) );
                   
                            break;

                        case Container container:

                            context.AddCommand(new ContainerRefreshItemCommand(container, Item) );

                            break;
                    }
                }

                resolve(context);
            } );            
        }
    }
}