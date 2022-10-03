using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ItemUpdateParentToContainerCommand : Command
    {
        public ItemUpdateParentToContainerCommand(Item item, Container toContainer)
        {
            Item = item;

            ToContainer = toContainer;
        }

        public Item Item { get; set; }
        
        public Container ToContainer { get; set; }

        public override void Execute(Context context)
        {
            switch (Item.Parent)
            {
                case Tile fromTile:

                    context.AddCommand(new TileRemoveItemCommand(fromTile, Item) ).Then(ctx =>
                    {
                        return ctx.AddCommand(new ContainerAddItemCommand(ToContainer, Item) );

                    } ).Then(ctx =>
                    {
                        OnComplete(ctx);
                    } );

                    break;

                case Inventory fromInventory:

                    context.AddCommand(new InventoryRemoveItemCommand(fromInventory, Item) ).Then(ctx =>
                    {
                        return ctx.AddCommand(new ContainerAddItemCommand(ToContainer, Item) );

                    } ).Then(ctx =>
                    {
                        OnComplete(ctx);
                    } );

                    break;

                case Container fromContainer:

                    context.AddCommand(new ContainerRemoveItemCommand(fromContainer, Item) ).Then(ctx =>
                    {
                        return ctx.AddCommand(new ContainerAddItemCommand(ToContainer, Item) );

                    } ).Then(ctx =>
                    {
                        OnComplete(ctx);
                    } );

                    break;
            }
        }
    }
}