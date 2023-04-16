using OpenTibia.Game.Commands;
using System;
using System.Diagnostics;

namespace OpenTibia.Game.CommandHandlers
{
    public abstract class CommandResultHandler<TResult, T> : ICommandResultHandler<TResult, T> where T : CommandResult<TResult>
    {
        public Context Context
        {
            get
            {
                return Context.Current;
            }
        }

        [DebuggerStepThrough]
        public PromiseResult<TResult> Handle(Func<PromiseResult<TResult>> next, CommandResult<TResult> command)
        {
            return Handle(next, (T)command);
        }

        public abstract PromiseResult<TResult> Handle(Func<PromiseResult<TResult>> next, T command);
    }
}