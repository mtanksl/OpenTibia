using OpenTibia.Game.EventHandlers;
using OpenTibia.Game.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game.Common.ServerObjects
{
    public class EventHandlerCollection : IEventHandlerCollection
    {
        private Dictionary<Type, Dictionary<Guid, IEventHandler> > types = new Dictionary<Type, Dictionary<Guid, IEventHandler> >();

        private Dictionary<GameEventArgs, Dictionary<Guid, IEventHandler> > objects = new Dictionary<GameEventArgs, Dictionary<Guid, IEventHandler> >();

        private class GuidItem
        {
            public GuidItem(Type type, GameEventArgs @object)
            {
                Type = type;

                Object = @object;
            }

            public Type Type { get; set; }

            public GameEventArgs Object { get; set; }
        }

        private Dictionary<Guid, GuidItem> guids = new Dictionary<Guid, GuidItem>();

        public int Count
        {
            get
            {
                return types.Count + objects.Count;
            }
        }

        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>

        public Guid Subscribe(Type type, Func<Context, object, Promise> execute)
        {
            return Subscribe(type, new InlineEventHandler(execute) );
        }

        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>

        public Guid Subscribe(Type type, IEventHandler eventHandler) 
        {
            if ( !typeof(GameEventArgs).IsAssignableFrom(type) )
            {
                throw new ArgumentException("Type must be of type GameEventArgs.");
            }

            if (eventHandler.IsDestroyed)
            {
                throw new InvalidOperationException("EventHandler is destroyed.");
            }

            Dictionary<Guid, IEventHandler> eventHandlers;

            if ( !types.TryGetValue(type, out eventHandlers) )
            {
                eventHandlers = new Dictionary<Guid, IEventHandler>();

                types.Add(type, eventHandlers);
            }

            eventHandlers.Add(eventHandler.Token, eventHandler);

            guids.Add(eventHandler.Token, new GuidItem(type, null) );

            return eventHandler.Token;
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

            guids.Add(eventHandler.Token, new GuidItem(typeof(T), null ) );

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

            guids.Add(eventHandler.Token, new GuidItem(null, e ) );

            return eventHandler.Token;
        }

        public bool Unsubscribe(Guid token)
        {
            GuidItem guiItem;

            if (guids.TryGetValue(token, out guiItem) )
            {
                guids.Remove(token);

                if (guiItem.Type != null)
                {
                    Dictionary<Guid, IEventHandler> eventHandlers;

                    if ( types.TryGetValue(guiItem.Type, out eventHandlers) )
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
                                    types.Remove(guiItem.Type);
                                }

                                return true;
                            }
                        }
                    }
                }
                else if (guiItem.Object != null)
                {
                    Dictionary<Guid, IEventHandler> eventHandlers;

                    if (objects.TryGetValue(guiItem.Object, out eventHandlers) )
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
                                    objects.Remove(guiItem.Object);
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
                foreach (var eventHandler in eventHandlers.Values.ToArray() )
                {
                    if ( !eventHandler.IsDestroyed)
                    {
                        yield return eventHandler;
                    }
                }
            }

            if ( objects.TryGetValue(e, out eventHandlers) )
            {
                foreach (var eventHandler in eventHandlers.Values.ToArray() )
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