using OpenTibia.Game.Commands;

namespace OpenTibia.Game.CommandHandlers
{
    public interface ICommandHandler
    {
        bool CanHandle(Command command, Context context);

        void Handle(Command command, Context context);
    }
}