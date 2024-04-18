using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using OpenTibia.Game.Components;
using System;

namespace OpenTibia.Game.Commands
{
    public class ItemDecayDestroyCommand : Command
    {
        public ItemDecayDestroyCommand(Item item, TimeSpan executeIn)
        {
            Item = item;

            ExecuteIn = executeIn;
        }

        public Item Item { get; set; }

        public TimeSpan ExecuteIn { get; set; }

        public override Promise Execute()
        {
            return Context.Server.GameObjectComponents.AddComponent(Item, new ItemDecayDelayBehaviour(ExecuteIn) ).Promise.Then( () =>
            { 
                return Context.AddCommand(new ItemDestroyCommand(Item) );
            } );
        }
    }
}