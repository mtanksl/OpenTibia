﻿using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;
using System;

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

        public override Promise Execute(Context context)
        {
            return Item.AddComponent(new DecayBehaviour(ExecuteInMilliseconds) ).Promise.Then(ctx =>
            { 
                return ctx.AddCommand(new ItemDestroyCommand(Item) );
            } );
        }
    }
}