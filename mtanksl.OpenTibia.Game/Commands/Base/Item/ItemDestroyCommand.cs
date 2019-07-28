using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ItemDestroyCommand : Command
    {
        public ItemDestroyCommand(Item item)
        {
            Item = item;
        }

        public Item Item { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            switch (Item.Container)
            {
                case Tile tile:

                    new TileRemoveItemCommand(tile, tile.GetIndex(Item) ).Execute(server, context);

                    break;

                case Inventory inventory:

                    new InventoryRemoveItemCommand(inventory, inventory.GetIndex(Item) ).Execute(server, context);

                    break;

                case Container container:

                    new ContainerRemoveItemCommand(container, container.GetIndex(Item) ).Execute(server, context);

                    break;
            }

            base.Execute(server, context);            
        }
    }
}
