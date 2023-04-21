using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using System;

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
            if (FluidItem.FluidType != FluidType)
            {
                FluidItem.FluidType = FluidType;

                switch (FluidItem.Parent)
                {
                    case Tile tile:

                        return Context.AddCommand(new TileRefreshItemCommand(tile, FluidItem) );

                    case Inventory inventory:

                        return Context.AddCommand(new InventoryRefreshItemCommand(inventory, FluidItem) );

                    case Container container:

                        return Context.AddCommand(new ContainerRefreshItemCommand(container, FluidItem) );

                    default:

                        throw new NotImplementedException();
                }
            }

            return Promise.Completed;
        }
    }
}