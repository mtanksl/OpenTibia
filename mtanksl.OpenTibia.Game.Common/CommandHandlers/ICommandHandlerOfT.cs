using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public interface ICommandHandler<T> : ICommandHandler where T : Command
    {
        Promise Handle(Func<Promise> next, T command);
    }
}