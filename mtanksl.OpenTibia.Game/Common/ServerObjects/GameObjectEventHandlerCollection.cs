using OpenTibia.Common.Objects;
using OpenTibia.Game.EventHandlers;
using OpenTibia.Game.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game.Common.ServerObjects
{
    public class GameObjectEventHandlerCollection : IGameObjectEventHandlerCollection
    {
        private Dictionary<uint, EventHandlerCollection> buckets = new Dictionary<uint, EventHandlerCollection>();

        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>

        public Guid Subscribe(GameObject eventSource, Type type, Func<Context, object, Promise> execute)
        {
            return Subscribe(eventSource, type, new InlineEventHandler(execute) );
        }

        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>

        public Guid Subscribe(GameObject eventSource, Type type, IEventHandler eventHandler) 
        {
            EventHandlerCollection eventHandlerCollection;

            if ( !buckets.TryGetValue(eventSource.Id, out eventHandlerCollection) )
            {
                eventHandlerCollection = new EventHandlerCollection();

                buckets.Add(eventSource.Id, eventHandlerCollection);
            }

            return eventHandlerCollection.Subscribe(type, eventHandler);
        }

        /// <exception cref="InvalidOperationException"></exception>

        public Guid Subscribe<T>(GameObject eventSource, Func<Context, T, Promise> execute) where T : GameEventArgs
        {
            return Subscribe<T>(eventSource, new InlineEventHandler<T>(execute) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public Guid Subscribe<T>(GameObject eventSource, IEventHandler<T> eventHandler) where T : GameEventArgs
        {
            EventHandlerCollection eventHandlerCollection;

            if ( !buckets.TryGetValue(eventSource.Id, out eventHandlerCollection) )
            {
                eventHandlerCollection = new EventHandlerCollection();

                buckets.Add(eventSource.Id, eventHandlerCollection);
            }

            return eventHandlerCollection.Subscribe(eventHandler);
        }

        /// <exception cref="InvalidOperationException"></exception>

        public Guid Subscribe<T>(GameObject eventSource, T e, Func<Context, T, Promise> execute) where T : GameEventArgs
        {
            return Subscribe<T>(eventSource, e, new InlineEventHandler<T>(execute) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public Guid Subscribe<T>(GameObject eventSource, T e, IEventHandler<T> eventHandler) where T : GameEventArgs
        {
            EventHandlerCollection eventHandlerCollection;

            if ( !buckets.TryGetValue(eventSource.Id, out eventHandlerCollection) )
            {
                eventHandlerCollection = new EventHandlerCollection();

                buckets.Add(eventSource.Id, eventHandlerCollection);
            }

            return eventHandlerCollection.Subscribe(e, eventHandler);
        }

        public bool Unsubscribe(GameObject eventSource, Guid token)
        {
            EventHandlerCollection eventHandlerCollection;

            if (buckets.TryGetValue(eventSource.Id, out eventHandlerCollection) )
            {
                if (eventHandlerCollection.Unsubscribe(token) )
                {
                    if (eventHandlerCollection.Count == 0)
                    {
                        buckets.Remove(eventSource.Id);
                    }

                    return true;
                }
            }

            return false;
        }

        public IEnumerable<IEventHandler> GetEventHandlers(GameObject eventSource, GameEventArgs e)
        {
            EventHandlerCollection eventHandlerCollection;

            if (buckets.TryGetValue(eventSource.Id, out eventHandlerCollection) )
            {
                return eventHandlerCollection.GetEventHandlers(e);
            }

            return Enumerable.Empty<IEventHandler>();
        }

        public void ClearEventHandlers(GameObject eventSource)
        {
            EventHandlerCollection eventHandlerCollection;

            if (buckets.TryGetValue(eventSource.Id, out eventHandlerCollection) )
            {
                eventHandlerCollection.Clear();

                buckets.Remove(eventSource.Id);
            }
        }
    }
}