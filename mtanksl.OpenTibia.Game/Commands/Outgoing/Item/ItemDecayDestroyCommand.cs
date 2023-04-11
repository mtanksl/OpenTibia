using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;

namespace OpenTibia.Game.Commands
{
    public class ItemDecayDestroyCommand : Command
    {
        public ItemDecayDestroyCommand(Item item, int executeInMilliseconds)
        {
            Item = item;

            ExecuteInMilliseconds = executeInMilliseconds;
        }

        public Item Item { get; set; }

        public int ExecuteInMilliseconds { get; set; }

        public override Promise Execute()
        {
            return Context.Server.Components.AddComponent(Item, new DelayBehaviour("Item_Decay", ExecuteInMilliseconds) ).Promise.Then( () =>
            { 
                return Context.AddCommand(new ItemDestroyCommand(Item) );
            } );
        }
    }
}