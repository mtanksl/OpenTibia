using OpenTibia.Common.Objects;
using System;

namespace OpenTibia.Game.Commands
{
    public class PlayerDestroyCommand : Command
    {
        public PlayerDestroyCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        public override Promise Execute(Context context)
        {
            return context.AddCommand(new TileRemoveCreatureCommand(Player.Tile, Player) ).Then(ctx =>
            {
                ctx.Server.PlayerFactory.Destroy(Player);
            } );
        }
    }
}