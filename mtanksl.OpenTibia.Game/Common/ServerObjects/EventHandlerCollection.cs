using OpenTibia.Game.EventHandlers;
using OpenTibia.Game.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game.Common.ServerObjects
{
    public class EventHandlerCollection : IEventHandlerCollection
    {
        private class GuidItem
        {
            public Type Type { get; set; }

            public GameEventArgs Object { get; set; }
        }

        private Dictionary<Type, Dictionary<Guid, IEventHandler> > types = new Dictionary<Type, Dictionary<Guid, IEventHandler> >();

        private Dictionary<GameEventArgs, Dictionary<Guid, IEventHandler> > objects = new Dictionary<GameEventArgs, Dictionary<Guid, IEventHandler> >();

        private Dictionary<Guid, GuidItem> guids = new Dictionary<Guid, GuidItem>();

        public int Count
        {
            get
            {
                return types.Count + objects.Count;
            }
        }

        /// <exception cref="InvalidOperationException"></exception>

        public Guid Subscribe<T>(Func<Context, T, Promise> execute) where T : GameEventArgs
        {
            return Subscribe(new InlineEventHandler<T>(execute) );
        }

        /// <exception cref="InvalidOperationException"></exception>
        
        public Guid Subscribe<T>(IEventHandler<T> eventHandler) where T : GameEventArgs
        {
            if (eventHandler.IsDestroyed)
            {
                throw new InvalidOperationException("EventHandler is destroyed.");
            }

            Dictionary<Guid, IEventHandler> eventHandlers;

            if ( !types.TryGetValue(typeof(T), out eventHandlers) )
            {
                eventHandlers = new Dictionary<Guid, IEventHandler>();

                types.Add(typeof(T), eventHandlers);
            }

            eventHandlers.Add(eventHandler.Token, eventHandler);

            guids.Add(eventHandler.Token, new GuidItem() { Type = typeof(T) } );

            return eventHandler.Token;
        }

        /// <exception cref="InvalidOperationException"></exception>

        public Guid Subscribe<T>(T e, Func<Context, T, Promise> execute) where T : GameEventArgs
        {
            return Subscribe(e, new InlineEventHandler<T>(execute) );
        }

        /// <exception cref="InvalidOperationException"></exception>
        
        public Guid Subscribe<T>(T e, IEventHandler<T> eventHandler) where T : GameEventArgs
        {
            if (eventHandler.IsDestroyed)
            {
                throw new InvalidOperationException("EventHandler is destroyed.");
            }

            Dictionary<Guid, IEventHandler> eventHandlers;

            if ( !objects.TryGetValue(e, out eventHandlers) )
            {
                eventHandlers = new Dictionary<Guid, IEventHandler>();

                objects.Add(e, eventHandlers);
            }

            eventHandlers.Add(eventHandler.Token, eventHandler);

            guids.Add(eventHandler.Token, new GuidItem() { Object = e } );

            return eventHandler.Token;
        }

        public bool Unsubscribe(Guid token)
        {
            GuidItem item;

            if (guids.TryGetValue(token, out item) )
            {
                guids.Remove(token);

                if (item.Type != null)
                {
                    Dictionary<Guid, IEventHandler> eventHandlers;

                    if ( types.TryGetValue(item.Type, out eventHandlers) )
                    {
                        IEventHandler eventHandler;

                        if (eventHandlers.TryGetValue(token, out eventHandler) )
                        {
                            if ( !eventHandler.IsDestroyed)
                            {
                                eventHandler.IsDestroyed = true;

                                eventHandlers.Remove(token);

                                if (eventHandlers.Count == 0)
                                {
                                    types.Remove(item.Type);
                                }

                                return true;
                            }
                        }
                    }
                }
                else if (item.Object != null)
                {
                    Dictionary<Guid, IEventHandler> eventHandlers;

                    if (objects.TryGetValue(item.Object, out eventHandlers) )
                    {
                        IEventHandler eventHandler;

                        if (eventHandlers.TryGetValue(token, out eventHandler) )
                        {
                            if ( !eventHandler.IsDestroyed)
                            {
                                eventHandler.IsDestroyed = true;

                                eventHandlers.Remove(token);

                                if (eventHandlers.Count == 0)
                                {
                                    objects.Remove(item.Object);
                                }

                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        public IEnumerable<IEventHandler> GetEventHandlers(GameEventArgs e)
        {
            Dictionary<Guid, IEventHandler> eventHandlers;

            if ( types.TryGetValue(e.GetType(), out eventHandlers) )
            {
                foreach (var eventHandler in eventHandlers.Values.ToList() )
                {
                    if ( !eventHandler.IsDestroyed)
                    {
                        yield return eventHandler;
                    }
                }
            }

            if ( objects.TryGetValue(e, out eventHandlers) )
            {
                foreach (var eventHandler in eventHandlers.Values.ToList() )
                {
                    if ( !eventHandler.IsDestroyed)
                    {
                        yield return eventHandler;
                    }
                }
            }
        }

        public void Clear()
        {
            guids.Clear();

            types.Clear();

            objects.Clear();
        }
    }
}