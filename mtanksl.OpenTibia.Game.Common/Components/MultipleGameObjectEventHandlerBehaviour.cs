using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using System;

namespace OpenTibia.Game.Components
{
    public class MultipleGameObjectEventHandlerBehaviour : Behaviour
    {
        private GameObject gameObject;

        private Type type;

        private Func<Context, object, Promise> execute;

        public MultipleGameObjectEventHandlerBehaviour(GameObject gameObject, Type type, Func<Context, object, Promise> execute)
        {
            this.gameObject = gameObject;

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
            key = Context.Server.GameObjectEventHandlers.Subscribe(gameObject, type, execute);
        }

        public override void Stop()
        {
            Context.Server.GameObjectEventHandlers.Unsubscribe(gameObject, key);
        }
    }
}