using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class PlayerMoveItemCommand : Command
    {
        public PlayerMoveItemCommand(Player player, Item item, IContainer toContainer, byte toIndex, byte count)
        {
            Player = player;

            Item = item;

            FromContainer = item.Parent;

            FromIndex = item.Parent.GetIndex(item);

            ToContainer = toContainer;

            ToIndex = toIndex;

            Count = count;
        }

        public Player Player { get; set; }

        public Item Item { get; set; }

        public IContainer FromContainer { get; set; }

        public byte FromIndex { get; set; }

        public IContainer ToContainer { get; set; }

        public byte ToIndex { get; set; }

        public byte Count { get; set; }

        public override void Execute(Context context)
        {
            // context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotMoveThisObject) );

            switch (FromContainer)
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
            
            base.Execute(context);
        }
    }
}