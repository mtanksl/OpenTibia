using OpenTibia.Game.Commands;

namespace OpenTibia.Game.CommandHandlers
{
    public abstract class CommandHandlerResult<TResult> : ICommandHandlerResult<TResult>
    {
        public abstract PromiseResult<TResult> Handle(Context context, ContextPromiseResultDelegate<TResult> next, CommandResult<TResult> command);
    }
}