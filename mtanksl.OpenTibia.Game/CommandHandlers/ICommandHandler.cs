using OpenTibia.Game.Commands;

namespace OpenTibia.Game.CommandHandlers
{
    public interface ICommandHandler
    {
        bool CanHandle(Command command, Server server);

        Command Handle(Command command, Server server);
    }
}