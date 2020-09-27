using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.CommandHandlers
{
    public class TeleportHandler : CommandHandler<PlayerSayCommand>
    {
        private int count;

        public override bool CanHandle(PlayerSayCommand command, Server server)
        {
            if (command.Message.StartsWith("/a") && command.Message.Contains(" ") && int.TryParse(command.Message.Split(' ')[1], out count) )
            {
                return true;
            }

            return false;
        }

        public override Command Handle(PlayerSayCommand command, Server server)
        {
            Tile toTile = server.Map.GetTile(command.Player.Tile.Position.Offset(command.Player.Direction, count) );

            if (toTile != null)
            {
                return new SequenceCommand(
                
                    new CreatureMoveCommand(command.Player, toTile),
                
                    new MagicEffectCommand(toTile.Position, MagicEffectType.Teleport) );
            }

            return new MagicEffectCommand(command.Player.Tile.Position, MagicEffectType.Puff);
        }
    }
}