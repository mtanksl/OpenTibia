using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using System;

namespace OpenTibia.Game.Components
{
    public class MultipleGameObjectEventHandlerBehaviour : Behaviour
    {
        private GameObject eventSource;

        private Type eventName;

        private Func<Context, object, Promise> execute;

        public MultipleGameObjectEventHandlerBehaviour(GameObject eventSource, Type eventName, Func<Context, object, Promise> execute)
        {
            this.eventSource = eventSource;

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
            key = Context.Server.GameObjectEventHandlers.Subscribe(eventSource, eventName, execute);
        }

        public override void Stop()
        {
            Context.Server.GameObjectEventHandlers.Unsubscribe(key);
        }
    }
}