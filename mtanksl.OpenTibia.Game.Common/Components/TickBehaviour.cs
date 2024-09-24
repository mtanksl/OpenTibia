using OpenTibia.Common.Objects;
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
            globalTick = Context.Server.EventHandlers.Subscribe(GlobalTickEventArgs.Instance[GameObject.Id % GlobalTickEventArgs.Instance.Length], (context, e) =>
            {
                return Update();
            } );
        }

        public abstract Promise Update();

        public override void Stop()
        {
            Context.Server.EventHandlers.Unsubscribe(globalTick);
        }
    }
}