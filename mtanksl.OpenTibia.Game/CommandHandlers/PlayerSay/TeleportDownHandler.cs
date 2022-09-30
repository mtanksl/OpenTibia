using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.CommandHandlers
{
    public class TeleportDownHandler : CommandHandler<PlayerSayCommand>
    {
        public override bool CanHandle(Context context, PlayerSayCommand command)
        {
            if (command.Message.StartsWith("/down") )
            {
                return true;
            }

            return false;
        }

        public override void Handle(Context context, PlayerSayCommand command)
        {
            Tile toTile = context.Server.Map.GetTile(command.Player.Tile.Position.Offset(0, 0, 1) );

            if (toTile != null)
            {
                context.AddCommand(new CreatureMoveCommand(command.Player, toTile), ctx =>
                {
                    ctx.AddCommand(new MagicEffectCommand(toTile.Position, MagicEffectType.Teleport) );

                    OnComplete(ctx);
                } );
            }
            else
            {
                context.AddCommand(new MagicEffectCommand(command.Player.Tile.Position, MagicEffectType.Puff) );

                OnComplete(context);
            }            
        }
    }
}