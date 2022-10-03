using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ItemUpdateParentToInventoryCommand : Command
    {
        public ItemUpdateParentToInventoryCommand(Item item, Inventory toInventory, byte slot)
        {
            Item = item;

            ToInventory = toInventory;

            Slot = slot;
        }

        public Item Item { get; set; }

        public Inventory ToInventory { get; set; }

        public byte Slot { get; set; }

        public override void Execute(Context context)
        {
            switch (Item.Parent)
            {
                case Tile fromTile:

                    context.AddCommand(new TileRemoveItemCommand(fromTile, Item) ).Then(ctx =>
                    {
                        return ctx.AddCommand(new InventoryAddItemCommand(ToInventory, Slot, Item) );

                    } ).Then(ctx =>
                    {
                        OnComplete(ctx);
                    } );

                    break;

                case Inventory fromInventory:

                    context.AddCommand(new InventoryRemoveItemCommand(fromInventory, Item) ).Then(ctx =>
                    {
                        return ctx.AddCommand(new InventoryAddItemCommand(ToInventory, Slot, Item) );

                    } ).Then(ctx =>
                    {
                        OnComplete(ctx);
                    } );

                    break;

                case Container fromContainer:

                    context.AddCommand(new ContainerRemoveItemCommand(fromContainer, Item) ).Then(ctx =>
                    {
                        return ctx.AddCommand(new InventoryAddItemCommand(ToInventory, Slot, Item) );

                    } ).Then(ctx =>
                    {
                        OnComplete(ctx);
                    } );

                    break;
            }
        }
    }
}