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

        public override void Execute(Context context)
        {
            if (Item.FluidType != FluidType)
            {
                Item.FluidType = FluidType;

                switch (Item.Parent)
                {
                    case Tile tile:

                        context.AddCommand(new TileUpdateItemCommand(tile, Item) ).Then(ctx =>
                        {
                            OnComplete(ctx);
                        } );
                  
                        break;

                    case Inventory inventory:

                        context.AddCommand(new InventoryUpdateItemCommand(inventory, Item) ).Then(ctx =>
                        {
                            OnComplete(ctx);
                        } );
                   
                        break;

                    case Container container:

                        context.AddCommand(new ContainerUpdateItemCommand(container, Item) ).Then(ctx =>
                        {
                            OnComplete(ctx);
                        } );

                        break;
                }
            }
            else
            {
                OnComplete(context);
            }
        }
    }
}