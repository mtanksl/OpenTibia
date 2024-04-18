using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

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

        public override Promise Execute()
        {
            if (Item is StackableItem stackableItem && stackableItem.Count > Count)
            {
                return Context.AddCommand(new StackableItemUpdateCountCommand(stackableItem, (byte)(stackableItem.Count - Count) ) );
            }

            return Context.AddCommand(new ItemDestroyCommand(Item) );
        }
    }
}