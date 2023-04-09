using OpenTibia.Game.Commands;

namespace OpenTibia.Game.CommandHandlers
{
    public abstract class CommandHandler<T> : CommandHandler where T : Command
    {
        public override Promise Handle(ContextPromiseDelegate next, Command command)
        {
            return Handle(next, (T)command);
        }

        public abstract Promise Handle(ContextPromiseDelegate next, T command);
    }
}