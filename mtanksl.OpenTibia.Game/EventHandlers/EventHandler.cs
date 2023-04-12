using OpenTibia.Game.Commands;
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

        public Guid Token { get; } = Guid.NewGuid();

        public abstract Promise Handle(object e);
    }
}