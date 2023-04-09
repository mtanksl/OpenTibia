using OpenTibia.Game.Commands;

namespace OpenTibia.Game.CommandHandlers
{
    public interface ICommandHandlerResult<TResult>
    {
        PromiseResult<TResult> Handle(ContextPromiseResultDelegate<TResult> next, CommandResult<TResult> command);
    }
}