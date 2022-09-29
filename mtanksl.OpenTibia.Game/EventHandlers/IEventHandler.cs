using System;

namespace OpenTibia.Game.EventHandlers
{
    public interface IEventHandler
    {
        Guid Token { get; }

        void Execute(object sender, object e);
    }
}