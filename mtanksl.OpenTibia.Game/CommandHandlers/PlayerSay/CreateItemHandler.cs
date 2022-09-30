using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.CommandHandlers
{
    public class CreateItemHandler : CommandHandler<PlayerSayCommand>
    {
        private ushort toOpenTibiaId;

        public override bool CanHandle(Context context, PlayerSayCommand command)
        {
            if (command.Message.StartsWith("/i") && command.Message.Contains(" ") && ushort.TryParse(command.Message.Split(' ')[1], out toOpenTibiaId) )
            {
                return true;
            }

            return false;
        }

        public override void Handle(Context context, PlayerSayCommand command)
        {
            Tile toTile = context.Server.Map.GetTile(command.Player.Tile.Position.Offset(command.Player.Direction) );

            if (toTile != null)
            {
                context.AddCommand(new ItemCreateCommand(toTile, toOpenTibiaId, 1) );

                context.AddCommand(new MagicEffectCommand(toTile.Position, MagicEffectType.BlueShimmer) );
            }
            else
            {
                context.AddCommand(new MagicEffectCommand(command.Player.Tile.Position, MagicEffectType.Puff) );
            }

            OnComplete(context);
        }
    }
}