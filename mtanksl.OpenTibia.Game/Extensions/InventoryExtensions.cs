using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Extensions
{
    public static class InventoryExtensions
    {
        public static Promise AddItem(this Inventory inventory, byte slot, Item item)
        {
            Context context = Context.Current;

            return context.AddCommand(new InventoryAddItemCommand(inventory, slot, item) );
        }

        public static PromiseResult<Item> CreateItem(this Inventory inventory, byte slot, ushort openTibiaId, byte count)
        {
            Context context = Context.Current;

            return context.AddCommand(new InventoryCreateItemCommand(inventory, slot, openTibiaId, count) );
        }

        public static PromiseResult<byte> RemoveItem(this Inventory inventory, Item item)
        {
            Context context = Context.Current;

            return context.AddCommand(new InventoryRemoveItemCommand(inventory, item) );
        }

        public static PromiseResult<byte> ReplaceItem(this Inventory inventory, Item fromItem, Item toItem)
        {
            Context context = Context.Current;

            return context.AddCommand(new InventoryReplaceItemCommand(inventory, fromItem, toItem) );
        }
    }
}