using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;

namespace OpenTibia.Game.Commands
{
    public class ItemDecayTransformCommand : CommandResult<Item>
    {
        public ItemDecayTransformCommand(Item item, int executeInMilliseconds, ushort openTibiaId, byte count)
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
            return context.Server.Components.AddComponent(Item, new DecayBehaviour(ExecuteInMilliseconds) ).Promise.Then(ctx =>
            { 
                return ctx.AddCommand(new ItemTransformCommand(Item, OpenTibiaId, Count) );
            } );
        }
    }
}