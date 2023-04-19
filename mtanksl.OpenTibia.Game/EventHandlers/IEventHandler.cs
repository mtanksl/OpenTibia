using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.EventHandlers
{
    public interface IEventHandler
    {
        bool IsDestroyed { get; set; }

        Guid Token { get; }

        Promise Handle(object e);
    }
}