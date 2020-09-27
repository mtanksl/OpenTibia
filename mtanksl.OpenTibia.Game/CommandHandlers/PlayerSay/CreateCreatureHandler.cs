using OpenTibia.Game.Commands;

namespace OpenTibia.Game.CommandHandlers
{
    public class CreateCreatureHandler : CommandHandler<PlayerSayCommand>
    {
        public override bool CanHandle(PlayerSayCommand command, Server server)
        {
            return false;
        }

        public override Command Handle(PlayerSayCommand command, Server server)
        {
            return command;
        }
    }
}