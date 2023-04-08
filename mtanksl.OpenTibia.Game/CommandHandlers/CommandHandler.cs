using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public abstract class CommandHandler : ICommandHandler
    {
        public abstract Promise Handle(Context context, Func<Context, Promise> next, Command command);
    }

    public abstract class CommandHandler<T> : CommandHandler where T : Command
    {
        public override Promise Handle(Context context, Func<Context, Promise> next, Command command)
        {
            return Handle(context, next, (T)command);
        }

        public abstract Promise Handle(Context context, Func<Context, Promise> next, T command);
    }
}