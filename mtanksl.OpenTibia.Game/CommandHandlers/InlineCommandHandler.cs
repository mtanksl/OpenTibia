using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class InlineCommandHandler<T> : CommandHandler<T> where T : Command
    {
        private Func<Context, Func<Context, Promise>, T, Promise> handle;

        public InlineCommandHandler(Func<Context, Func<Context, Promise>, T, Promise> handle)
        {
            this.handle = handle;
        }

        public override Promise Handle(Context context, Func<Context, Promise> next, T command)
        {
            return handle(context, next, command);
        }
    }
}