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

        public override void Execute(Context context)
        {
            switch (Item.Parent)
            {
                case Tile tile:

                    context.AddCommand(new TileRemoveItemCommand(tile, Item) ).Then(ctx =>
                    {
                        ctx.Server.ItemFactory.Destroy(Item);

                        OnComplete(ctx);
                    } );
                  
                    break;

                case Inventory inventory:

                    context.AddCommand(new InventoryRemoveItemCommand(inventory, Item) ).Then(ctx =>
                    {
                        ctx.Server.ItemFactory.Destroy(Item);

                        OnComplete(ctx);
                    } );
                   
                    break;

                case Container container:

                    context.AddCommand(new ContainerRemoveItemCommand(container, Item) ).Then(ctx =>
                    {
                        ctx.Server.ItemFactory.Destroy(Item);

                        OnComplete(ctx);
                    } );

                    break;
            }
        }
    }
}