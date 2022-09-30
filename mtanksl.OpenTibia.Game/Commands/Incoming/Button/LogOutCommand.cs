using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class LogOutCommand : Command
    {
        public LogOutCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        public override void Execute(Context context)
        {
            Tile fromTile = Player.Tile;

            context.AddCommand(new PlayerDestroyCommand(Player) ).Then(ctx =>
            {
                ctx.AddCommand(new ShowMagicEffectCommand(fromTile.Position, MagicEffectType.Puff) );
                

                ctx.Disconnect(Player.Client.Connection);

                OnComplete(ctx);
            } );
        }
    }
}