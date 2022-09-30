using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public interface ICommandHandlerResult<TResult>
    {
        Action<Context, TResult> ContinueWith { get; set; }

        bool CanHandle(Context context, CommandResult<TResult> command);

        void Handle(Context context, CommandResult<TResult> command);
    }
}