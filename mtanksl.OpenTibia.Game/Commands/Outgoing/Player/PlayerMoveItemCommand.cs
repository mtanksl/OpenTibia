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
            return Promise.Run( (resolve, reject) =>
            {
                switch (Item.Parent)
                {
                    case Tile fromTile:

                        context.AddCommand(new TileRemoveItemCommand(fromTile, Item) );

                        break;

                    case Inventory fromInventory:

                        context.AddCommand(new InventoryRemoveItemCommand(fromInventory, Item) );

                        break;

                    case Container fromContainer:

                        context.AddCommand(new ContainerRemoveItemCommand(fromContainer, Item) );

                        break;
                }

                switch (ToContainer)
                {
                    case Tile toTile:

                        context.AddCommand(new TileAddItemCommand(toTile, Item) );

                        break;

                    case Inventory toInventory:

                        context.AddCommand(new InventoryAddItemCommand(toInventory, ToIndex, Item) );

                        break;

                    case Container toContainer:

                        context.AddCommand(new ContainerAddItemCommand(toContainer, Item) );
                        
                        break;
                }

                resolve(context);
            } );
        }
    }
}