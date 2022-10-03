using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ItemUpdateParentToTileCommand : Command
    {
        public ItemUpdateParentToTileCommand(Item item, Tile toTile)
        {
            Item = item;

            ToTile = toTile;
        }

        public Item Item { get; set; }

        public Tile ToTile { get; set; }

        public override void Execute(Context context)
        {
            switch (Item.Parent)
            {
                case Tile fromTile:

                    context.AddCommand(new TileRemoveItemCommand(fromTile, Item) ).Then(ctx =>
                    {
                        return ctx.AddCommand(new TileAddItemCommand(ToTile, Item) );

                    } ).Then(ctx =>
                    {
                        OnComplete(ctx);
                    } );

                    break;

                case Inventory fromInventory:

                    context.AddCommand(new InventoryRemoveItemCommand(fromInventory, Item) ).Then(ctx =>
                    {
                        return ctx.AddCommand(new TileAddItemCommand(ToTile, Item) );

                    } ).Then(ctx =>
                    {
                        OnComplete(ctx);
                    } );

                    break;

                case Container fromContainer:

                    context.AddCommand(new ContainerRemoveItemCommand(fromContainer, Item) ).Then(ctx =>
                    {
                        return ctx.AddCommand(new TileAddItemCommand(ToTile, Item) );

                    } ).Then(ctx =>
                    {
                        OnComplete(ctx);
                    } );

                    break;
            }
        }
    }
}