using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public interface ICommandHandler
    {
        bool Canceled { get; set; }

        Guid Token { get; }

        Promise Handle(Func<Promise> next, Command command);
    }   
}