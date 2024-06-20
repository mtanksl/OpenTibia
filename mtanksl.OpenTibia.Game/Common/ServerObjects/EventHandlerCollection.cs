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

            return eventHandler.Token;
        }

        public bool Unsubscribe<T>(Guid token) where T : GameEventArgs
        {
            Dictionary<Guid, IEventHandler> eventHandlers;

            if ( types.TryGetValue(typeof(T), out eventHandlers) )
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
                            types.Remove(typeof(T) );
                        }

                        return true;
                    }
                }
            }

            return false;
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

            return eventHandler.Token;
        }

        public bool Unsubscribe<T>(T e, Guid token) where T : GameEventArgs
        {
            Dictionary<Guid, IEventHandler> eventHandlers;

            if (objects.TryGetValue(e, out eventHandlers) )
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
                            objects.Remove(e);
                        }

                        return true;
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
            types.Clear();

            objects.Clear();
        }
    }
}