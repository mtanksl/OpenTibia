using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ItemDecrementCountCommand : Command
    {
        public ItemDecrementCountCommand(StackableItem item, byte count)
        {
            Item = item;

            Count = count;
        }

        public StackableItem Item { get; set; }

        public byte Count { get; set; }

        public override void Execute(Context context)
        {
            if (Item.Count > Count)
            {
                context.AddCommand(new StackableItemUpdateCountCommand(Item, (byte)(Item.Count - Count) ) ).Then(ctx =>
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