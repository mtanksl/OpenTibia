using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class PlayerMoveItemCommand : Command
    {
        public PlayerMoveItemCommand(Player player, Item item, IContainer toContainer, byte toIndex, byte count)
        {
            Player = player;

            Item = item;

            ToContainer = toContainer;

            ToIndex = toIndex;

            Count = count;
        }

        public Player Player { get; set; }

        public Item Item { get; set; }

        public IContainer ToContainer { get; set; }

        public byte ToIndex { get; set; }

        public byte Count { get; set; }

        public override void Execute(Context context)
        {
            switch (ToContainer)
            {
                case Tile toTile:

                    context.AddCommand(new ItemMoveToTileCommand(Item, toTile) ).Then(ctx =>
                    {
                        OnComplete(ctx);
                    } );

                    break;

                case Inventory toInventory:

                    context.AddCommand(new ItemMoveToInventoryCommand(Item, toInventory, ToIndex) ).Then(ctx =>
                    {
                        OnComplete(ctx);
                    } );

                    break;

                case Container toContainer:

                    context.AddCommand(new ItemMoveToContainerCommand(Item, toContainer) ).Then(ctx =>
                    {
                        OnComplete(ctx);
                    } );

                    break;
            }
        }
    }
}