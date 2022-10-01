using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.CommandHandlers
{
    public class CreateMonsterHandler : CommandHandler<PlayerSayCommand>
    {
        private string name;

        public override bool CanHandle(Context context, PlayerSayCommand command)
        {
            if (command.Message.StartsWith("/m") && command.Message.Contains(" ") )
            {
                name = command.Message.Split(' ')[1];

                return true;
            }

            return false;
        }

        public override void Handle(Context context, PlayerSayCommand command)
        {
            Tile toTile = context.Server.Map.GetTile(command.Player.Tile.Position.Offset(command.Player.Direction) );

            if (toTile != null)
            {
                context.AddCommand(new MonsterCreateCommand(toTile, name) ).Then( (ctx, monster) =>
                {
                    return ctx.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.BlueShimmer) );

                } ).Then(ctx =>
                {
                    OnComplete(ctx);
                } );
            }
            else
            {
                context.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.Puff) );
            }
        }
    }
}