using OpenTibia.Game.Common;
using System;

namespace OpenTibia.Game.EventHandlers
{
    public abstract class EventHandler : IEventHandler
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

        public abstract Promise Handle(object e);
    }
}