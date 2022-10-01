using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ItemDecrementCountCommand : Command
    {
        public ItemDecrementCountCommand(Item item, byte count)
        {
            Item = item;

            Count = count;
        }

        public Item Item { get; set; }

        public byte Count { get; set; }

        public override void Execute(Context context)
        {
            if (Item is StackableItem stackableItem && stackableItem.Count > Count)
            {
                context.AddCommand(new ItemUpdateCommand(Item, (byte)(stackableItem.Count - Count) ) ).Then(ctx =>
                {
                    OnComplete(ctx);
                } );
            }
            else
            {
                context.AddCommand(new ItemDestroyCommand(Item) ).Then(ctx =>
                {
                    OnComplete(ctx);
                } );
            }
        }
    }
}