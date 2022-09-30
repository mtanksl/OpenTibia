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
                case Container container:

                    context.AddCommand(new ContainerRemoveItemCommand(container, Item), ctx =>
                    {
                        ctx.Server.ItemFactory.Destroy(Item);

                        OnComplete(ctx);
                    } );

                    break;

                case Inventory inventory:

                    context.AddCommand(new InventoryRemoveItemCommand(inventory, Item), ctx =>
                    {
                        ctx.Server.ItemFactory.Destroy(Item);

                        OnComplete(ctx);
                    } );
                   
                    break;

                case Tile tile:

                    context.AddCommand(new TileRemoveItemCommand(tile, Item), ctx =>
                    {
                        ctx.Server.ItemFactory.Destroy(Item);

                        OnComplete(ctx);
                    } );
                  
                    break;
            }
        }
    }
}