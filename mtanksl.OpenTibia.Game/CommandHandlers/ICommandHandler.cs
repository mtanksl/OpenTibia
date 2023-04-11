using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public interface ICommandHandler
    {
        Promise Handle(Func<Promise> next, Command command);
    }
}