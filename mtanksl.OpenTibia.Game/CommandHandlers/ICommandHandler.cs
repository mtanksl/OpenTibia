using OpenTibia.Game.Commands;

namespace OpenTibia.Game.CommandHandlers
{
    public interface ICommandHandler
    {
        bool CanHandle(Command command, Context context);

        Command Handle(Command command, Context context);
    }
}