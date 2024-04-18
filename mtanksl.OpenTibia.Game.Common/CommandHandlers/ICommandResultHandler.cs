using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public interface ICommandResultHandler<TResult>
    {
        bool IsDestroyed { get; set; }

        Guid Token { get; }

        PromiseResult<TResult> Handle(Func<PromiseResult<TResult>> next, CommandResult<TResult> command);
    }
}