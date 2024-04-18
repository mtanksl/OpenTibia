using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;

namespace OpenTibia.Game.Extensions
{
    public static class ContainerExtensions
    {
        /// <exception cref="InvalidOperationException"></exception>

        public static Promise AddItem(this Container container, Item item)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new ContainerAddItemCommand(container, item) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static PromiseResult<Item> CreateItem(this Container container, ushort openTibiaId, byte count)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new ContainerCreateItemCommand(container, openTibiaId, count) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise RemoveItem(this Container container, Item item)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new ContainerRemoveItemCommand(container, item) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise ReplaceItem(this Container container, Item fromItem, Item toItem)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new ContainerReplaceItemCommand(container, fromItem, toItem) );
        }
    }
}