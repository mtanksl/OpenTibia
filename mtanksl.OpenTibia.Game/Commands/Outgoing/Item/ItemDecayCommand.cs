using OpenTibia.Common.Objects;
using System;

namespace OpenTibia.Game.Commands
{
    public class ItemDecayCommand : CommandResult<Item>
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

        public override PromiseResult<Item> Execute(Context context)
        {
            return Promise.Delay(context, Constants.ItemDecaySchedulerEvent(Item), ExecuteInMilliseconds).Then(ctx =>
            {
                return ctx.AddCommand(new ItemTransformCommand(Item, OpenTibiaId, Count) );
            } );
        }
    }
}