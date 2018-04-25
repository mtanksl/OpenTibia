using System;
using System.Collections.Generic;

namespace OpenTibia
{
    public class EventBus
    {
        private interface IHandler
        {
            Guid Token { get; }

            void Process(GameEvent evento);
        }

        private class Handler<T> : IHandler where T : GameEvent
        {
            public Handler(Guid token, Action<T> action)
            {
                this.token = token;

                this.action = action;
            }

            private Guid token;

            public Guid Token
            {
                get
                {
                    return token;
                }
            }

            private Action<T> action;

            public void Process(GameEvent evento)
            {
                action(evento as T);
            }
        }
        
        private readonly object sync = new object();

            private Dictionary< Type, List<IHandler> > typeHandlers = new Dictionary< Type, List<IHandler> >();

        public Guid Subscribe<T>(Action<T> action) where T : GameEvent
        {
            lock (sync)
            {
                List<IHandler> handlers;

                if ( !typeHandlers.TryGetValue(typeof(T), out handlers) )
                {
                    handlers = new List<IHandler>();

                    typeHandlers.Add(typeof(T), handlers);
                }

                Guid token = Guid.NewGuid();

                handlers.Add( new Handler<T>(token, action) );

                return token;
            }
        }

        public void Unsubscribe(Guid token)
        {
            lock (sync)
            {
                foreach (var typeHandler in typeHandlers)
                {
                    List<IHandler> handlers = typeHandler.Value;

                    foreach (var handler in handlers)
                    {
                        if (handler.Token == token)
                        {
                            handlers.Remove(handler);

                            return;
                        }
                    }
                }
            }
        }

        public void Publish<T>(T evento) where T : GameEvent
        {
            lock (sync)
            {
                List<IHandler> handlers;

                if ( typeHandlers.TryGetValue(typeof(T), out handlers) )
                {
                    foreach (var handler in handlers)
                    {
                        handler.Process(evento);
                    }
                }
            }
        }
    }
}