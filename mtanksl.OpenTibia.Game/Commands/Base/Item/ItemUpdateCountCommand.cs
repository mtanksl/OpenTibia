using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ItemUpdateCountCommand : Command
    {
        public ItemUpdateCountCommand(StackableItem item, byte count)
        {
            Item = item;

            Count = count;
        }

        public StackableItem Item { get; set; }

        public byte Count { get; set; }

        public override void Execute(Context context)
        {
            if (Item.Count != Count)
            {
                Item.Count = Count;

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