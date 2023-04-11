using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class InlineCommandHandler<T> : CommandHandler<T> where T : Command
    {
        private Func<Context, Func<Promise>, T, Promise> handle;

        public InlineCommandHandler(Func<Context, Func<Promise>, T, Promise> handle)
        {
            this.handle = handle;
        }

        public override Promise Handle(Func<Promise> next, T command)
        {
            return handle(Context, next, command);
        }
    }
}