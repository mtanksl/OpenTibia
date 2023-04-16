using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public interface ICommandResultHandler<TResult, T> : ICommandResultHandler<TResult> where T : CommandResult<TResult>
    {
        PromiseResult<TResult> Handle(Func<PromiseResult<TResult>> next, T command);
    }
}