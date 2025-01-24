using OpenTibia.Game.Common;
using System;

namespace OpenTibia.Game.Components
{
    public class MultipleEventHandlerBehaviour : Behaviour
    {
        private Type type;

        private Func<Context, object, Promise> execute;

        public MultipleEventHandlerBehaviour(Type type, Func<Context, object, Promise> execute)
        {
            this.type = type;

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
            key = Context.Server.EventHandlers.Subscribe(type, execute);
        }

        public override void Stop()
        {
            Context.Server.EventHandlers.Unsubscribe(key);
        }
    }
}