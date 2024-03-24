using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.CommandHandlers
{
    public class GamemasterCommandHandler : CommandHandlerCommandHandlerCollection<PlayerSayCommand>
    {
        public override bool CanHandle(PlayerSayCommand command)
        {
            return command.Player.Rank == Rank.Gamemaster && command.Message.StartsWith("/");
        }
    }
}