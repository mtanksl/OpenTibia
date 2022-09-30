using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class PlayerDestroyCommand : Command
    {
        public PlayerDestroyCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        public override void Execute(Context context)
        {
            context.AddCommand(new TileRemoveCreatureCommand(Player.Tile, Player), ctx =>
            {
                ctx.Server.PlayerFactory.Destroy(Player);

                OnComplete(ctx);
            } );
        }
    }
}