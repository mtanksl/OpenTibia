using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using System;

namespace OpenTibia.Game.Components
{
    public abstract class TickBehaviour : Behaviour
    {
        private Guid globalTick;

        public override void Start()
        {
            globalTick = Context.Server.EventHandlers.Subscribe<GlobalTickEventArgs>( (context, e) =>
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
            Context.Server.EventHandlers.Unsubscribe<GlobalTickEventArgs>(globalTick);
        }
    }
}