using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.Extensions
{
    public static class StackableItemExtensions
    {
        /// <exception cref="InvalidOperationException"></exception>

        public static Promise UpdateCount(this StackableItem item, byte count)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new StackableItemUpdateCountCommand(item, count) );
        }
    }
}