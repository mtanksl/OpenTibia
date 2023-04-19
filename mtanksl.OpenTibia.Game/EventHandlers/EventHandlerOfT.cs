using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;
using System;
using System.Diagnostics;

namespace OpenTibia.Game.EventHandlers
{
    public abstract class EventHandler<T> : IEventHandler<T> where T : GameEventArgs
    {
        public Context Context
        {
            get
            {
                return Context.Current;
            }
        }

        public bool IsDestroyed { get; set; }

        public Guid Token { get; } = Guid.NewGuid();

        [DebuggerStepThrough]
        public Promise Handle(object e)
        {
            return Handle( (T)e);
        }

        public abstract Promise Handle(T e);
    }
}