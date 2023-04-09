using System;

namespace OpenTibia.Game.EventHandlers
{
    public abstract class EventHandler : IEventHandler
    {
        public Context context
        {
            get
            {
                return Context.Current;
            }
        }

        public Guid Token { get; } = Guid.NewGuid();

        public abstract void Handle(object e);
    }
}