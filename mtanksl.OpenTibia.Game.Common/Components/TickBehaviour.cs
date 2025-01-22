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
            globalTick = Context.Server.EventHandlers.Subscribe(GlobalTickEventArgs.Instance(GameObject.Id), (context, e) =>
            {
                return Update(e);
            } );
        }

        public abstract Promise Update(GlobalTickEventArgs e);

        public override void Stop()
        {
            Context.Server.EventHandlers.Unsubscribe(globalTick);
        }
    }
}