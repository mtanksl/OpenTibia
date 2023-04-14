using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public abstract class CommandResultHandler<TResult> : ICommandResultHandler<TResult>
    {
        public Context Context
        {
            get
            {
                return Context.Current;
            }
        }

        public abstract PromiseResult<TResult> Handle(Func<PromiseResult<TResult>> next, CommandResult<TResult> command);
    }
}