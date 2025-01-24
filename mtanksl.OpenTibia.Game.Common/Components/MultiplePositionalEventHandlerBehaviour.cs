using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using System;

namespace OpenTibia.Game.Components
{
    public class MultiplePositionalEventHandlerBehaviour : Behaviour
    {
        private GameObject observer;

        private Type eventName;

        private Func<Context, object, Promise> execute;

        public MultiplePositionalEventHandlerBehaviour(GameObject observer, Type eventName, Func<Context, object, Promise> execute)
        {
            this.observer = observer;

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
            key = Context.Server.PositionalEventHandlers.Subscribe(observer, eventName, execute);
        }

        public override void Stop()
        {
            Context.Server.PositionalEventHandlers.Unsubscribe(key);
        }
    }
}