using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public interface ICommandHandlerResult<TResult>
    {
        bool CanHandle(Context context, CommandResult<TResult> command);

        void Handle(Context context, CommandResult<TResult> command);

        Action<Context, TResult> Continuation { get; set; }
    }
}