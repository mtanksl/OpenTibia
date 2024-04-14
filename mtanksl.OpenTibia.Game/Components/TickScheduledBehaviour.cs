﻿using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;
using System;

namespace OpenTibia.Game.Components
{
    public abstract class TickScheduledBehaviour : Behaviour
    {
        private Guid globalTickEventArgs;

        public override void Start()
        {
            globalTickEventArgs = Context.Server.EventHandlers.Subscribe<GlobalTickEventArgs>( (context, e) =>
            {
                if (e.Index == GameObject.Id % 10)
                {
                    return Update();
                }

                return Promise.Completed;
            } );
        }

        public abstract Promise Update();

        public override void Stop()
        {
            Context.Server.EventHandlers.Unsubscribe<GlobalTickEventArgs>(globalTickEventArgs);
        }
    }
}