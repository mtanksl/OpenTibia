using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Diagnostics;

namespace OpenTibia.Game.CommandHandlers
{
    public class InlineCommandHandler<T> : CommandHandler<T> where T : Command
    {
        private Func<Context, Func<Promise>, T, Promise> handle;

        public InlineCommandHandler(Func<Context, Func<Promise>, T, Promise> handle)
        {
            this.handle = handle;
        }

        [DebuggerStepThrough]
        public override Promise Handle(Func<Promise> next, T command)
        {
            return handle(Context, next, command);
        }
    }
}