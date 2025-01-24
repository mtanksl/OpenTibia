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
        private Dictionary<Type, Dictionary<GameObject, Dictionary<Guid, IEventHandler> > > types = new Dictionary<Type, Dictionary<GameObject, Dictionary<Guid, IEventHandler> > >();

        private Dictionary<GameEventArgs, Dictionary<GameObject, Dictionary<Guid, IEventHandler> > > objects = new Dictionary<GameEventArgs, Dictionary<GameObject, Dictionary<Guid, IEventHandler> > >();

        private class GuidItem
        {
            public GuidItem(GameObject eventSource, Type type, GameEventArgs @object)
            {
                EventSource = eventSource;

                Type = type;

                Object = @object;
            }

            public GameObject EventSource { get; }

            public Type Type { get; }

            public GameEventArgs Object { get; }
        }

        private Dictionary<Guid, GuidItem> guids = new Dictionary<Guid, GuidItem>();

        private Dictionary<GameObject, HashSet<Guid> > gameObjects = new Dictionary<GameObject, HashSet<Guid> >();

        public int Count
        {
            get
            {
                return types.Count + objects.Count;
            }
        }

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
            if ( !typeof(GameEventArgs).IsAssignableFrom(type) )
            {
                throw new ArgumentException("Type must be of type GameEventArgs.");
            }

            if (eventHandler.IsDestroyed)
            {
                throw new InvalidOperationException("EventHandler is destroyed.");
            }

            Dictionary<GameObject, Dictionary<Guid, IEventHandler> > gameObjectEventHandlers;

            if ( !types.TryGetValue(type, out gameObjectEventHandlers) )
            {
                gameObjectEventHandlers = new Dictionary<GameObject, Dictionary<Guid, IEventHandler> >();

                types.Add(type, gameObjectEventHandlers);
            }

            Dictionary<Guid, IEventHandler> eventHandlers;

            if ( !gameObjectEventHandlers.TryGetValue(eventSource, out eventHandlers) )
            {
                eventHandlers = new Dictionary<Guid, IEventHandler>();

                gameObjectEventHandlers.Add(eventSource, eventHandlers);
            }

            eventHandlers.Add(eventHandler.Token, eventHandler);

            guids.Add(eventHandler.Token, new GuidItem(eventSource, type, null) );

            HashSet<Guid> tokens;

            if ( !gameObjects.TryGetValue(eventSource, out tokens) )
            {
                tokens = new HashSet<Guid>();

                gameObjects.Add(eventSource, tokens);
            }

            tokens.Add(eventHandler.Token);

            return eventHandler.Token;
        }

        /// <exception cref="InvalidOperationException"></exception>

        public Guid Subscribe<T>(GameObject eventSource, Func<Context, T, Promise> execute) where T : GameEventArgs
        {
            return Subscribe(eventSource, new InlineEventHandler<T>(execute) );
        }

        /// <exception cref="InvalidOperationException"></exception>
        
        public Guid Subscribe<T>(GameObject eventSource, IEventHandler<T> eventHandler) where T : GameEventArgs
        {
            if (eventHandler.IsDestroyed)
            {
                throw new InvalidOperationException("EventHandler is destroyed.");
            }

            Dictionary<GameObject, Dictionary<Guid, IEventHandler>> gameObjectEventHandlers;

            if ( !types.TryGetValue(typeof(T), out gameObjectEventHandlers))
            {
                gameObjectEventHandlers = new Dictionary<GameObject, Dictionary<Guid, IEventHandler>>();

                types.Add(typeof(T), gameObjectEventHandlers);
            }

            Dictionary<Guid, IEventHandler> eventHandlers;

            if ( !gameObjectEventHandlers.TryGetValue(eventSource, out eventHandlers))
            {
                eventHandlers = new Dictionary<Guid, IEventHandler>();

                gameObjectEventHandlers.Add(eventSource, eventHandlers);
            }

            eventHandlers.Add(eventHandler.Token, eventHandler);

            guids.Add(eventHandler.Token, new GuidItem(eventSource, typeof(T), null) );

            HashSet<Guid> tokens;

            if ( !gameObjects.TryGetValue(eventSource, out tokens) )
            {
                tokens = new HashSet<Guid>();

                gameObjects.Add(eventSource, tokens);
            }

            tokens.Add(eventHandler.Token);

            return eventHandler.Token;
        }

        /// <exception cref="InvalidOperationException"></exception>

        public Guid Subscribe<T>(GameObject eventSource, T e, Func<Context, T, Promise> execute) where T : GameEventArgs
        {
            return Subscribe(eventSource, e, new InlineEventHandler<T>(execute) );
        }

        /// <exception cref="InvalidOperationException"></exception>
        
        public Guid Subscribe<T>(GameObject eventSource, T e, IEventHandler<T> eventHandler) where T : GameEventArgs
        {
            if (eventHandler.IsDestroyed)
            {
                throw new InvalidOperationException("EventHandler is destroyed.");
            }

            Dictionary<GameObject, Dictionary<Guid, IEventHandler> > gameObjectEventHandlers;

            if ( !objects.TryGetValue(e, out gameObjectEventHandlers) )
            {
                gameObjectEventHandlers = new Dictionary<GameObject, Dictionary<Guid, IEventHandler> >();

                objects.Add(e, gameObjectEventHandlers);
            }

            Dictionary<Guid, IEventHandler> eventHandlers;

            if ( !gameObjectEventHandlers.TryGetValue(eventSource, out eventHandlers) )
            {
                eventHandlers = new Dictionary<Guid, IEventHandler>();

                gameObjectEventHandlers.Add(eventSource, eventHandlers);
            }

            eventHandlers.Add(eventHandler.Token, eventHandler);

            guids.Add(eventHandler.Token, new GuidItem(eventSource, null, e) );

            HashSet<Guid> tokens;

            if ( !gameObjects.TryGetValue(eventSource, out tokens) )
            {
                tokens = new HashSet<Guid>();

                gameObjects.Add(eventSource, tokens);
            }

            tokens.Add(eventHandler.Token);

            return eventHandler.Token;
        }

        public bool Unsubscribe(Guid token)
        {
            GuidItem guidItem;

            if (guids.TryGetValue(token, out guidItem) )
            {
                guids.Remove(token);

                HashSet<Guid> tokens;

                if (gameObjects.TryGetValue(guidItem.EventSource, out tokens) )
                {
                    tokens.Remove(token);

                    if (tokens.Count == 0)
                    {
                        gameObjects.Remove(guidItem.EventSource);
                    }

                    if (guidItem.Type != null)
                    {
                        Dictionary<GameObject, Dictionary<Guid, IEventHandler> > gameObjectEventHandlers;

                        if (types.TryGetValue(guidItem.Type, out gameObjectEventHandlers) )
                        {
                            Dictionary<Guid, IEventHandler> eventHandlers;

                            if (gameObjectEventHandlers.TryGetValue(guidItem.EventSource, out eventHandlers) )
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
                                            gameObjectEventHandlers.Remove(guidItem.EventSource);

                                            if (gameObjectEventHandlers.Count == 0)
                                            {
                                                types.Remove(guidItem.Type);
                                            }
                                        }

                                        return true;
                                    }
                                }
                            }
                        }
                    }
                    else if (guidItem.Object != null)
                    {
                        Dictionary<GameObject, Dictionary<Guid, IEventHandler> > gameObjectEventHandlers;

                        if (objects.TryGetValue(guidItem.Object, out gameObjectEventHandlers) )
                        {
                            Dictionary<Guid, IEventHandler> eventHandlers;

                            if (gameObjectEventHandlers.TryGetValue(guidItem.EventSource, out eventHandlers) )
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
                                            gameObjectEventHandlers.Remove(guidItem.EventSource);

                                            if (gameObjectEventHandlers.Count == 0)
                                            {
                                                objects.Remove(guidItem.Object);
                                            }
                                        }

                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        public IEnumerable<IEventHandler> GetEventHandlers(GameObject eventSource, GameEventArgs e)
        {
            Dictionary<GameObject, Dictionary<Guid, IEventHandler> > gameObjectEventHandlers;

            if (types.TryGetValue(e.GetType(), out gameObjectEventHandlers) )
            {
                Dictionary<Guid, IEventHandler> eventHandlers;

                if (gameObjectEventHandlers.TryGetValue(eventSource, out eventHandlers) )
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

            if (objects.TryGetValue(e, out gameObjectEventHandlers) )
            {
                Dictionary<Guid, IEventHandler> eventHandlers;

                if (gameObjectEventHandlers.TryGetValue(eventSource, out eventHandlers) )
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
        }

        public void ClearEventHandlers(GameObject eventSource)
        {
            HashSet<Guid> tokens;

            if (gameObjects.TryGetValue(eventSource, out tokens) )
            {
                foreach (var token in tokens.ToArray() )
                {
                    Unsubscribe(token);
                }
            }
        }
    }
}