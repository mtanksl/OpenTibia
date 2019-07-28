using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ItemMoveCommand : Command
    {
        public ItemMoveCommand(Item item, IContainer toContainer, byte toIndex)
        {
            Item = item;

            ToContainer = toContainer;

            ToIndex = toIndex;
        }

        public Item Item { get; set; }

        public IContainer ToContainer { get; set; }

        public byte ToIndex { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            switch (Item.Container)
            {
                case Tile fromTile:

                    new TileRemoveItemCommand(fromTile, fromTile.GetIndex(Item) ).Execute(server, context);

                    break;

                case Inventory fromInventory:

                    new InventoryRemoveItemCommand(fromInventory, fromInventory.GetIndex(Item) ).Execute(server, context);

                    break;

                case Container fromContainer:

                    new ContainerRemoveItemCommand(fromContainer, fromContainer.GetIndex(Item) ).Execute(server, context);

                    break;
            }

            switch (ToContainer)
            {
                case Tile toTile:

                    new TileAddItemCommand(toTile, Item).Execute(server, context);

                    break;

                case Inventory toInventory:

                    new InventoryAddItemCommand(toInventory, ToIndex, Item).Execute(server, context);

                    break;

                case Container toContainer:

                    new ContainerAddItemCommand(toContainer, Item).Execute(server, context);

                    break;
            }

            base.Execute(server, context);
        }
    }
}
