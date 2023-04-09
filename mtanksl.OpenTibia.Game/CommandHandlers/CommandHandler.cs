using OpenTibia.Game.Commands;

namespace OpenTibia.Game.CommandHandlers
{
    public abstract class CommandHandler : ICommandHandler
    {
        public Context context
        {
            get
            {
                return Context.Current;
            }
        }

        public abstract Promise Handle(ContextPromiseDelegate next, Command command);
    }
}