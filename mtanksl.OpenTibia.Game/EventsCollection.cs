using OpenTibia.Common.Events;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game
{
    public class EventsCollection
    {
        private interface IEventHandler
        {
            Guid Token { get; }

            void Execute(object sender, object e);
        }

        private class EventHandler<T> : IEventHandler where T : GameEventArgs
        {
            private Action<object, T> callback;

            public EventHandler(Action<object, T> callback)
            {
                this.callback = callback;
            }

            public Guid Token { get; } = Guid.NewGuid();

            public void Execute(object sender, object e)
            {
                callback(sender, (T)e);
            }
        }

        private Dictionary<Type, Dictionary<Guid, IEventHandler> > types = new Dictionary<Type, Dictionary<Guid, IEventHandler> >();

        public Guid Subscribe<T>(Action<object, T> callback) where T : GameEventArgs
        {
            if ( !types.TryGetValue(typeof(T), out var handlers) )
            {
                handlers = new Dictionary<Guid, IEventHandler>();

                types.Add(typeof(T), handlers);
            }

            var handler = new EventHandler<T>(callback);

            handlers.Add(handler.Token, handler);

            return handler.Token;
        }

        public void Unsubscribe<T>(Guid token) where T : GameEventArgs
        {
            if ( types.TryGetValue(typeof(T), out var handlers) )
            {
                handlers.Remove(token);

                if (handlers.Count == 0)
                {
                    types.Remove(typeof(T) );
                }
            }
        }

        public void Publish(object sender, GameEventArgs e)
        {
            if ( types.TryGetValue(e.GetType(), out var handlers) )
            {
                foreach (var handler in handlers.Values)
                {
                    handler.Execute(sender, e);
                }
            }
        }
    }
}