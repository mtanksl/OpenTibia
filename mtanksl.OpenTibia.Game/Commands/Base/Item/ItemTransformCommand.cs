using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class ItemTransformCommand : Command
    {
        public ItemTransformCommand(Item item, ushort openTibiaId, byte count)
        {
            Item = item;

            OpenTibiaId = openTibiaId;

            Count = count;
        }

        public Item Item { get; set; }

        public ushort OpenTibiaId { get; set; }

        public byte Count { get; set; }

        public override void Execute(Context context)
        {
            Item toItem = context.Server.ItemFactory.Create(OpenTibiaId);

            if (toItem is StackableItem stackableItem)
            {
                stackableItem.Count = Count;
            }
            else if (toItem is FluidItem fluidItem)
            {
                fluidItem.FluidType = (FluidType)Count;
            }

            if (toItem != null)
            {
                Command command = null;

                switch (Item.Container)
                {
                    case Tile tile:

                        command = new TileReplaceItemCommand(tile, Item, toItem);

                        break;

                    case Inventory inventory:

                        command = new InventoryReplaceItemCommand(inventory, Item, toItem);

                        break;

                    case Container container:

                        command = new ContainerReplaceItemCommand(container, Item, toItem);

                        break;
                }

                command = context.TransformCommand(command);

                command.Completed += (s, e) =>
                {
                    context.Server.ItemFactory.Destroy(Item);

                    base.OnCompleted(e.Context);
                };

                command.Execute(context);
            }
        }
    }
}