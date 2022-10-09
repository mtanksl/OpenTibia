using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public interface ICommandHandlerResult<TResult>
    {
        PromiseResult<TResult> Handle(Context context, Func<Context, PromiseResult<TResult> > next, CommandResult<TResult> command);
    }
}