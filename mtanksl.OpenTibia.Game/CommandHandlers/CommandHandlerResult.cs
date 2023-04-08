using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public abstract class CommandHandlerResult<TResult> : ICommandHandlerResult<TResult>
    {
        public abstract PromiseResult<TResult> Handle(Context context, Func<Context, PromiseResult<TResult> > next, CommandResult<TResult> command);
    }

    public abstract class CommandHandlerResult<T, TResult> : CommandHandlerResult<TResult> where T : CommandResult<TResult>
    {
        public override PromiseResult<TResult> Handle(Context context, Func<Context, PromiseResult<TResult> > next, CommandResult<TResult> command)
        {
            return Handle(context, next, (T)command);
        }

        public abstract PromiseResult<TResult> Handle(Context context, Func<Context, PromiseResult<TResult> > next, T command);
    }
}