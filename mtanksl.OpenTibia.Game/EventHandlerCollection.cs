using OpenTibia.Common.Events;
using OpenTibia.Game.EventHandlers;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game
{
    public class EventHandlerCollection
    {
        private Dictionary<Type, Dictionary<Guid, IEventHandler> > types = new Dictionary<Type, Dictionary<Guid, IEventHandler> >();

        public Guid Subscribe<T>(EventHandlers.EventHandler<T> eventHandler) where T : GameEventArgs
        {
            Dictionary<Guid, IEventHandler> handlers;

            if ( !types.TryGetValue(typeof(T), out handlers) )
            {
                handlers = new Dictionary<Guid, IEventHandler>();

                types.Add(typeof(T), handlers);
            }

            handlers.Add(eventHandler.Token, eventHandler);

            return eventHandler.Token;
        }

        public void Unsubscribe<T>(Guid token) where T : GameEventArgs
        {
            Dictionary<Guid, IEventHandler> handlers;

            if ( types.TryGetValue(typeof(T), out handlers) )
            {
                handlers.Remove(token);

                if (handlers.Count == 0)
                {
                    types.Remove(typeof(T) );
                }
            }
        }

        public void Publish(Context context, GameEventArgs e)
        {
            if ( types.TryGetValue(e.GetType(), out var handlers) )
            {
                foreach (var handler in handlers.Values)
                {
                    handler.Handle(context, e);
                }
            }
        }
    }
}