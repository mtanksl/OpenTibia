using System;

namespace OpenTibia.Game.EventHandlers
{
    public abstract class EventHandler : IEventHandler
    {
        public Guid Token { get; } = Guid.NewGuid();

        public abstract void Handle(Context context, object e);
    }
}