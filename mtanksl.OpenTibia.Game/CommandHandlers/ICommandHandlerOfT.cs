using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public interface ICommandHandler<T> : ICommandHandler where T : Command
    {
        Promise Handle(Func<Promise> next, T command);
    }
}