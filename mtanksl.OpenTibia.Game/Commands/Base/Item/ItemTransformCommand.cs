using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ItemTransformCommand : Command
    {
        public ItemTransformCommand(Item item, ushort openTibiaId)
        {
            Item = item;

            OpenTibiaId = openTibiaId;
        }

        public Item Item { get; set; }

        public ushort OpenTibiaId { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            Item toItem = server.ItemFactory.Create(OpenTibiaId);

            if (toItem != null)
            {
                switch (Item.Container)
                {
                    case Tile tile:

                        new TileReplaceItemCommand(tile, tile.GetIndex(Item), toItem).Execute(server, context);

                        break;

                    case Inventory inventory:

                        new InventoryReplaceItemCommand(inventory, inventory.GetIndex(Item), toItem).Execute(server, context);

                        break;

                    case Container container:

                        new ContainerReplaceItemCommand(container, container.GetIndex(Item), toItem).Execute(server, context);

                        break;
                }

                base.Execute(server, context);
            }
        }
    }
}
