using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.CommandHandlers
{
    public class TeleportUpHandler : CommandHandler<PlayerSayCommand>
    {
        public override bool CanHandle(PlayerSayCommand command, Server server)
        {
            if (command.Message.StartsWith("/up") )
            {
                return true;
            }

            return false;
        }

        public override Command Handle(PlayerSayCommand command, Server server)
        {
            Tile toTile = server.Map.GetTile(command.Player.Tile.Position.Offset(0, 0, -1) );

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