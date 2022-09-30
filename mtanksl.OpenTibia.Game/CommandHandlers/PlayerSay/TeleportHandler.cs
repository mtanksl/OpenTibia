using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.CommandHandlers
{
    public class TeleportHandler : CommandHandler<PlayerSayCommand>
    {
        private int count;

        public override bool CanHandle(Context context, PlayerSayCommand command)
        {
            if (command.Message.StartsWith("/a") && command.Message.Contains(" ") && int.TryParse(command.Message.Split(' ')[1], out count) )
            {
                return true;
            }

            return false;
        }

        public override void Handle(Context context, PlayerSayCommand command)
        {
            Tile toTile = context.Server.Map.GetTile(command.Player.Tile.Position.Offset(command.Player.Direction, count) );

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