using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.EventHandlers
{
    public interface IEventHandler
    {
        Guid Token { get; }

        Promise Handle(object e);
    }
}