using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class InlineCommandHandlerResult<T, TResult> : CommandHandlerResult<T, TResult> where T : CommandResult<TResult>
    {
        private Func<Context, ContextPromiseResultDelegate<TResult>, T, PromiseResult<TResult> > handle;

        public InlineCommandHandlerResult(Func<Context, ContextPromiseResultDelegate<TResult>, T, PromiseResult<TResult> > handle)
        {
            this.handle = handle;
        }

        public override PromiseResult<TResult> Handle(ContextPromiseResultDelegate<TResult> next, T command)
        {
            return handle(context, next, command);
        }
    }
}