using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public interface ICommandResultHandler<TResult>
    {
        PromiseResult<TResult> Handle(Func<PromiseResult<TResult>> next, CommandResult<TResult> command);
    }
}