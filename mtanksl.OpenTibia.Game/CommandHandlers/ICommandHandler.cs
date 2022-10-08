using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public interface ICommandHandler
    {
        Promise Handle(Context context, Func<Context, Promise> next, Command command);
    }
}