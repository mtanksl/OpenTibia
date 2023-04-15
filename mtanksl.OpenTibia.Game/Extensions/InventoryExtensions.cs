using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.Extensions
{
    public static class InventoryExtensions
    {
        /// <exception cref="InvalidOperationException"></exception>

        public static Promise AddItem(this Inventory inventory, byte slot, Item item)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new InventoryAddItemCommand(inventory, slot, item) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static PromiseResult<Item> CreateItem(this Inventory inventory, byte slot, ushort openTibiaId, byte count)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new InventoryCreateItemCommand(inventory, slot, openTibiaId, count) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise RemoveItem(this Inventory inventory, Item item)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new InventoryRemoveItemCommand(inventory, item) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise ReplaceItem(this Inventory inventory, Item fromItem, Item toItem)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new InventoryReplaceItemCommand(inventory, fromItem, toItem) );
        }
    }
}