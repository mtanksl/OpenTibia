using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public abstract class CommandHandler<T> : CommandHandler where T : Command
    {
        public override Promise Handle(Func<Promise> next, Command command)
        {
            return Handle(next, (T)command);
        }

        public abstract Promise Handle(Func<Promise> next, T command);
    }
}