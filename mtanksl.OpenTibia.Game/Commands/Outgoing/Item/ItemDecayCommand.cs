using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ItemDecayCommand : Command
    {
        public ItemDecayCommand(Item item, int executeInMilliseconds, ushort openTibiaId, byte count)
        {
            Item = item;

            ExecuteInMilliseconds = executeInMilliseconds;

            OpenTibiaId = openTibiaId;

            Count = count;
        }

        public Item Item { get; set; }

        public int ExecuteInMilliseconds { get; set; }

        public ushort OpenTibiaId { get; set; }

        public byte Count { get; set; }

        public override void Execute(Context context)
        {
            Promise.Delay(context, Constants.ItemDecaySchedulerEvent(Item), ExecuteInMilliseconds).Then(ctx =>
            {
                ctx.AddCommand(new ItemTransformCommand(Item, OpenTibiaId, Count) );
            } );

            OnComplete(context);
        }
    }
}