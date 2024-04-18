using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public interface ICommandHandler
    {
        bool IsDestroyed { get; set; }

        Guid Token { get; }

        Promise Handle(Func<Promise> next, Command command);
    }   
}