﻿using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using System;

namespace OpenTibia.Game.Components
{
    public class ItemDecayDestroyBehaviour : Behaviour
    {
        private Guid globalDecay;

        public override void Start()
        {
            Item item = (Item)GameObject;

            globalDecay = Context.Server.EventHandlers.Subscribe(GlobalDecayEventArgs.Instance(item.Id), (context, e) =>
            {
                item.DurationInMilliseconds -= e.Ticks;

                if (item.DurationInMilliseconds <= 0)
                {
                    return Context.AddCommand(new ItemDestroyCommand(item) );
                }

                return Promise.Completed;
            } );           
        }

        public override void Stop()
        {
            Context.Server.EventHandlers.Unsubscribe(globalDecay);
        }
    }
}