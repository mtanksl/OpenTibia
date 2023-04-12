using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class PlayerMoveItemCommand : Command
    {
        public PlayerMoveItemCommand(Player player, Item item, IContainer toContainer, byte toIndex, byte count, bool pathfinding)
        {
            Player = player;

            Item = item;

            ToContainer = toContainer;

            ToIndex = toIndex;

            Count = count;

            Pathfinding = pathfinding;
        }

        public Player Player { get; set; }

        public Item Item { get; set; }

        public IContainer ToContainer { get; set; }

        public byte ToIndex { get; set; }

        public byte Count { get; set; }

        public bool Pathfinding { get; set; }

        public override Promise Execute()
        {
            switch (Item.Parent)
            {
                case Tile fromTile:

                    Context.AddCommand(new TileRemoveItemCommand(fromTile, Item) );

                    break;

                case Inventory fromInventory:

                    Context.AddCommand(new InventoryRemoveItemCommand(fromInventory, Item) );

                    break;

                case Container fromContainer:

                    Context.AddCommand(new ContainerRemoveItemCommand(fromContainer, Item) );

                    break;
            }

            switch (ToContainer)
            {
                case Tile toTile:

                    Context.AddCommand(new TileAddItemCommand(toTile, Item) );

                    break;

                case Inventory toInventory:

                    Context.AddCommand(new InventoryAddItemCommand(toInventory, ToIndex, Item) );

                    break;

                case Container toContainer:

                    Context.AddCommand(new ContainerAddItemCommand(toContainer, Item) );
                        
                    break;
            }

            return Promise.Completed;
        }
    }
}