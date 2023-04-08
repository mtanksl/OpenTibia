using OpenTibia.Game.Commands;

namespace OpenTibia.Game.CommandHandlers
{
    public abstract class CommandHandlerResult<T, TResult> : CommandHandlerResult<TResult> where T : CommandResult<TResult>
    {
        public override PromiseResult<TResult> Handle(Context context, ContextPromiseResultDelegate<TResult> next, CommandResult<TResult> command)
        {
            return Handle(context, next, (T)command);
        }

        public abstract PromiseResult<TResult> Handle(Context context, ContextPromiseResultDelegate<TResult> next, T command);
    }
}