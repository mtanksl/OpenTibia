using OpenTibia.Game.Commands;

namespace OpenTibia.Game.CommandHandlers
{
    public abstract class CommandHandlerResult<TResult> : ICommandHandlerResult<TResult>
    {
        public Context context
        {
            get
            {
                return Context.Current;
            }
        }

        public abstract PromiseResult<TResult> Handle(ContextPromiseResultDelegate<TResult> next, CommandResult<TResult> command);
    }
}