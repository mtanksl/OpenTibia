using OpenTibia.Game.Commands;

namespace OpenTibia.Game.CommandHandlers
{
    public abstract class CommandHandlerResult<T, TResult> : CommandHandlerResult<TResult> where T : CommandResult<TResult>
    {
        public override PromiseResult<TResult> Handle(ContextPromiseResultDelegate<TResult> next, CommandResult<TResult> command)
        {
            return Handle(next, (T)command);
        }

        public abstract PromiseResult<TResult> Handle(ContextPromiseResultDelegate<TResult> next, T command);
    }
}