using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.CommandHandlers
{
    public class CreateItemHandler : CommandHandler<PlayerSayCommand>
    {
        private ushort toOpenTibiaId;

        public override bool CanHandle(PlayerSayCommand command, Server server)
        {
            if (command.Message.StartsWith("/i") && command.Message.Contains(" ") && ushort.TryParse(command.Message.Split(' ')[1], out toOpenTibiaId) )
            {
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

                   new TileCreateItemCommand(toTile, toOpenTibiaId, 1),

                   new MagicEffectCommand(toTile.Position, MagicEffectType.BlueShimmer) );
            }

            return new MagicEffectCommand(command.Player.Tile.Position, MagicEffectType.Puff);
        }
    }
}