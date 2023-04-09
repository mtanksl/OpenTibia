using OpenTibia.Game.Commands;

namespace OpenTibia.Game.CommandHandlers
{
    public interface ICommandHandler
    {
        Promise Handle(ContextPromiseDelegate next, Command command);
    }
}