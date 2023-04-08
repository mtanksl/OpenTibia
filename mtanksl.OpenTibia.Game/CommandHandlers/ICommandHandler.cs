using OpenTibia.Game.Commands;

namespace OpenTibia.Game.CommandHandlers
{
    public interface ICommandHandler
    {
        Promise Handle(Context context, ContextPromiseDelegate next, Command command);
    }
}