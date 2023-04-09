using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class InlineCommandHandler<T> : CommandHandler<T> where T : Command
    {
        private Func<Context, ContextPromiseDelegate, T, Promise> handle;

        public InlineCommandHandler(Func<Context, ContextPromiseDelegate, T, Promise> handle)
        {
            this.handle = handle;
        }

        public override Promise Handle(ContextPromiseDelegate next, T command)
        {
            return handle(context, next, command);
        }
    }
}