using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Extensions
{
    public static class StackableItemExtensions
    {
        public static Promise UpdateCount(this StackableItem item, byte count)
        {
            Context context = Context.Current;

            return context.AddCommand(new StackableItemUpdateCountCommand(item, count) );
        }
    }
}