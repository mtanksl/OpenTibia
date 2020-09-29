using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class ItemUpdateFluidTypeCommand : Command
    {
        public ItemUpdateFluidTypeCommand(FluidItem item, FluidType fluidType)
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

                Command command = null;

                switch (Item.Container)
                {
                    case Tile tile:

                        command = new TileUpdateItemCommand(tile, Item);

                        break;

                    case Inventory inventory:

                        command = new InventoryUpdateItemCommand(inventory, Item);

                        break;

                    case Container container:

                        command = new ContainerUpdateItemCommand(container, Item);

                        break;
                }

                command = context.TransformCommand(command);

                command.Completed += (s, e) =>
                {
                    base.OnCompleted(e.Context);
                };

                command.Execute(context);
            }
        }
    }
}