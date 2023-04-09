using System;

namespace OpenTibia.Game.EventHandlers
{
    public interface IEventHandler
    {
        Guid Token { get; }

        void Handle(object e);
    }
}