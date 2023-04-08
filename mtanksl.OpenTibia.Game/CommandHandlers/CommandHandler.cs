using OpenTibia.Game.Commands;

namespace OpenTibia.Game.CommandHandlers
{
    public abstract class CommandHandler : ICommandHandler
    {
        public abstract Promise Handle(Context context, ContextPromiseDelegate next, Command command);
    }
}