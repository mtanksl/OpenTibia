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

        /// <exception cref="InvalidOperationException"></exception>

        public Guid Subscribe<T>(GameObject gameObject, Func<Context, T, Promise> execute) where T : GameEventArgs
        {
            return Subscribe<T>(gameObject, new InlineEventHandler<T>(execute) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public Guid Subscribe<T>(GameObject gameObject, IEventHandler<T> eventHandler) where T : GameEventArgs
        {
            EventHandlerCollection eventHandlerCollection;

            if ( !buckets.TryGetValue(gameObject.Id, out eventHandlerCollection) )
            {
                eventHandlerCollection = new EventHandlerCollection();

                buckets.Add(gameObject.Id, eventHandlerCollection);
            }

            return eventHandlerCollection.Subscribe(eventHandler);
        }

        public bool Unsubscribe<T>(GameObject gameObject, Guid token) where T : GameEventArgs
        {
            EventHandlerCollection eventHandlerCollection;

            if (buckets.TryGetValue(gameObject.Id, out eventHandlerCollection) )
            {
                if (eventHandlerCollection.Unsubscribe<T>(token) )
                {
                    if (eventHandlerCollection.Count == 0)
                    {
                        buckets.Remove(gameObject.Id);
                    }

                    return true;
                }
            }

            return false;
        }

        /// <exception cref="InvalidOperationException"></exception>

        public Guid Subscribe<T>(GameObject gameObject, T e, Func<Context, T, Promise> execute) where T : GameEventArgs
        {
            return Subscribe<T>(gameObject, e, new InlineEventHandler<T>(execute) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public Guid Subscribe<T>(GameObject gameObject, T e, IEventHandler<T> eventHandler) where T : GameEventArgs
        {
            EventHandlerCollection eventHandlerCollection;

            if ( !buckets.TryGetValue(gameObject.Id, out eventHandlerCollection) )
            {
                eventHandlerCollection = new EventHandlerCollection();

                buckets.Add(gameObject.Id, eventHandlerCollection);
            }

            return eventHandlerCollection.Subscribe(e, eventHandler);
        }

        public bool Unsubscribe<T>(GameObject gameObject, T e, Guid token) where T : GameEventArgs
        {
            EventHandlerCollection eventHandlerCollection;

            if (buckets.TryGetValue(gameObject.Id, out eventHandlerCollection) )
            {
                if (eventHandlerCollection.Unsubscribe<T>(e, token) )
                {
                    if (eventHandlerCollection.Count == 0)
                    {
                        buckets.Remove(gameObject.Id);
                    }

                    return true;
                }
            }

            return false;
        }

        public IEnumerable<IEventHandler> GetEventHandlers(GameObject gameObject, GameEventArgs e)
        {
            EventHandlerCollection eventHandlerCollection;

            if (buckets.TryGetValue(gameObject.Id, out eventHandlerCollection) )
            {
                return eventHandlerCollection.GetEventHandlers(e);
            }

            return Enumerable.Empty<IEventHandler>();
        }

        public void ClearEventHandlers(GameObject gameObject)
        {
            EventHandlerCollection eventHandlerCollection;

            if (buckets.TryGetValue(gameObject.Id, out eventHandlerCollection) )
            {
                eventHandlerCollection.Clear();

                buckets.Remove(gameObject.Id);
            }
        }
    }
}