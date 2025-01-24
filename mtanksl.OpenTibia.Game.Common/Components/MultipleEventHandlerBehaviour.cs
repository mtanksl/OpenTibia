using OpenTibia.Game.Common;
using System;

namespace OpenTibia.Game.Components
{
    public class MultipleEventHandlerBehaviour : Behaviour
    {
        private Type eventName;

        private Func<Context, object, Promise> execute;

        public MultipleEventHandlerBehaviour(Type eventName, Func<Context, object, Promise> execute)
        {
            this.eventName = eventName;

            this.execute = execute;
        }

        private Guid key;

        public Guid Key
        {
            get
            {
                return key;
            }
        }

        public override void Start()
        {
            key = Context.Server.EventHandlers.Subscribe(eventName, execute);
        }

        public override void Stop()
        {
            Context.Server.EventHandlers.Unsubscribe(key);
        }
    }
}