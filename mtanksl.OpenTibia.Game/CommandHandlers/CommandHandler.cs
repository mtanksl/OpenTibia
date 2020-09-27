using OpenTibia.Game.Commands;

namespace OpenTibia.Game.CommandHandlers
{
    public abstract class CommandHandler<T> : ICommandHandler where T : Command
    {
        public abstract bool CanHandle(T command, Server server);

        public abstract Command Handle(T command, Server server);

        public bool CanHandle(Command command, Server server)
        {
            return CanHandle( (T)command, server);
        }

        public Command Handle(Command command, Server server)
        {
            return Handle( (T)command, server);
        }
    }
}