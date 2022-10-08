using OpenTibia.Common.Objects;
using System;

namespace OpenTibia.Game.Commands
{
    public class ItemDecrementCommand : Command
    {
        public ItemDecrementCommand(Item item, byte count)
        {
            Item = item;

            Count = count;
        }

        public Item Item { get; set; }

        public byte Count { get; set; }

        public override Promise Execute(Context context)
        {
            if (Item is StackableItem stackableItem && stackableItem.Count > Count)
            {
                return context.AddCommand(new StackableItemUpdateCountCommand(stackableItem, (byte)(stackableItem.Count - Count) ) );
            }
            else
            {
                return context.AddCommand(new ItemDestroyCommand(Item) );
            }
        }
    }
}