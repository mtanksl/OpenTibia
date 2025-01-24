using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.EventHandlers;
using OpenTibia.Game.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game.Common.ServerObjects
{
    public class PositionalEventHandlerCollection : IPositionalEventHandlerCollection
    {
        private IServer server;

        public PositionalEventHandlerCollection(IServer server)
        {
            this.server = server;
        }

        private Dictionary<uint, EventHandlerCollection> buckets = new Dictionary<uint, EventHandlerCollection>();

        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>

        public Guid Subscribe(GameObject observer, Type type, Func<Context, object, Promise> execute)
        {
            return Subscribe(observer, type, new InlineEventHandler(execute) );
        }

        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>

        public Guid Subscribe(GameObject observer, Type type, IEventHandler eventHandler) 
        {
            EventHandlerCollection eventHandlerCollection;

            if ( !buckets.TryGetValue(observer.Id, out eventHandlerCollection) )
            {
                eventHandlerCollection = new EventHandlerCollection();

                buckets.Add(observer.Id, eventHandlerCollection);
            }

            return eventHandlerCollection.Subscribe(type, eventHandler);
        }

        /// <exception cref="InvalidOperationException"></exception>

        public Guid Subscribe<T>(GameObject observer, Func<Context, T, Promise> execute) where T : GameEventArgs
        {
            return Subscribe<T>(observer, new InlineEventHandler<T>(execute) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public Guid Subscribe<T>(GameObject observer, IEventHandler<T> eventHandler) where T : GameEventArgs
        {
            EventHandlerCollection eventHandlerCollection;

            if ( !buckets.TryGetValue(observer.Id, out eventHandlerCollection) )
            {
                eventHandlerCollection = new EventHandlerCollection();

                buckets.Add(observer.Id, eventHandlerCollection);
            }

            return eventHandlerCollection.Subscribe(eventHandler);
        }

        /// <exception cref="InvalidOperationException"></exception>

        public Guid Subscribe<T>(GameObject observer, T e, Func<Context, T, Promise> execute) where T : GameEventArgs
        {
            return Subscribe<T>(observer, e, new InlineEventHandler<T>(execute) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public Guid Subscribe<T>(GameObject observer, T e, IEventHandler<T> eventHandler) where T : GameEventArgs
        {
            EventHandlerCollection eventHandlerCollection;

            if ( !buckets.TryGetValue(observer.Id, out eventHandlerCollection) )
            {
                eventHandlerCollection = new EventHandlerCollection();

                buckets.Add(observer.Id, eventHandlerCollection);
            }

            return eventHandlerCollection.Subscribe(e, eventHandler);
        }

        public bool Unsubscribe(GameObject observer, Guid token)
        {
            EventHandlerCollection eventHandlerCollection;

            if (buckets.TryGetValue(observer.Id, out eventHandlerCollection) )
            {
                if (eventHandlerCollection.Unsubscribe(token) )
                {
                    if (eventHandlerCollection.Count == 0)
                    {
                        buckets.Remove(observer.Id);
                    }

                    return true;
                }
            }

            return false;
        }

        public IEnumerable<IEventHandler> GetEventHandlers(GameObject eventSource, GameEventArgs e)
        {
            Position position = null;

            switch (eventSource)
            {
                case Item item:

                    switch (item.Root() )
                    {
                        case Tile tile:

                            position = tile.Position;

                            break;

                        case Inventory inventory:

                            position = inventory.Player.Tile.Position;

                            break;

                        case Safe safe:

                            position = safe.Player.Tile.Position;

                            break;
                    }

                    break;

                case Creature creature:

                    position = creature.Tile.Position;

                    break;
            }

            foreach (var observer in server.Map.GetObserversOfTypeCreature(position) )
            {
                if (observer.Tile.Position.CanSee(position) )
                {
                    EventHandlerCollection eventHandlerCollection;

                    if (buckets.TryGetValue(observer.Id, out eventHandlerCollection) )
                    {
                        foreach (var eventHandler in eventHandlerCollection.GetEventHandlers(e) )
                        {
                            yield return eventHandler;
                        };
                    }
                }
            }
        }

        public void ClearEventHandlers(GameObject observer)
        {
            EventHandlerCollection eventHandlerCollection;

            if (buckets.TryGetValue(observer.Id, out eventHandlerCollection) )
            {
                eventHandlerCollection.Clear();

                buckets.Remove(observer.Id);
            }
        }
    }
}