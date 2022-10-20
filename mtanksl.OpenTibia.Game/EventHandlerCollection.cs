using OpenTibia.Game.Events;
using OpenTibia.Game.EventHandlers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game
{
    public class EventHandlerCollection
    {
        private Dictionary<Type, Dictionary<Guid, IEventHandler> > types = new Dictionary<Type, Dictionary<Guid, IEventHandler> >();

        public Guid Subscribe<T>(EventHandlers.EventHandler<T> eventHandler) where T : GameEventArgs
        {
            Dictionary<Guid, IEventHandler> eventHandlers;

            if ( !types.TryGetValue(typeof(T), out eventHandlers) )
            {
                eventHandlers = new Dictionary<Guid, IEventHandler>();

                types.Add(typeof(T), eventHandlers);
            }

            eventHandlers.Add(eventHandler.Token, eventHandler);

            return eventHandler.Token;
        }

        public void Unsubscribe<T>(Guid token) where T : GameEventArgs
        {
            Dictionary<Guid, IEventHandler> eventHandlers;

            if ( types.TryGetValue(typeof(T), out eventHandlers) )
            {
                eventHandlers.Remove(token);

                if (eventHandlers.Count == 0)
                {
                    types.Remove(typeof(T) );
                }
            }
        }

        public IEnumerable<IEventHandler> Get(GameEventArgs e)
        {
            if ( types.TryGetValue(e.GetType(), out var eventHandlers) )
            {
                return eventHandlers.Values;
            }

            return Enumerable.Empty<IEventHandler>();
        }
    }
}