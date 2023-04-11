using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public abstract class CommandHandlerResult<T, TResult> : CommandHandlerResult<TResult> where T : CommandResult<TResult>
    {
        public override PromiseResult<TResult> Handle(Func<PromiseResult<TResult>> next, CommandResult<TResult> command)
        {
            return Handle(next, (T)command);
        }

        public abstract PromiseResult<TResult> Handle(Func<PromiseResult<TResult>> next, T command);
    }
}