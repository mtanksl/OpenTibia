using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.CommandHandlers
{
    public class CreateCreatureHandler : CommandHandler<PlayerSayCommand>
    {
        private string name;

        public override bool CanHandle(PlayerSayCommand command, Server server)
        {
            if (command.Message.StartsWith("/m") && command.Message.Contains(" ") )
            {
                name = command.Message.Split(' ')[1];

                return true;
            }

            return false;
        }

        public override Command Handle(PlayerSayCommand command, Server server)
        {
            Tile toTile = server.Map.GetTile(command.Player.Tile.Position.Offset(command.Player.Direction) );

            if (toTile != null)
            {
                return new SequenceCommand(

                    new TileCreateCreatureCommand(toTile, name),

                    new MagicEffectCommand(toTile.Position, MagicEffectType.BlueShimmer) );
            }
            
            return new MagicEffectCommand(command.Player.Tile.Position, MagicEffectType.Puff);
        }
    }
}