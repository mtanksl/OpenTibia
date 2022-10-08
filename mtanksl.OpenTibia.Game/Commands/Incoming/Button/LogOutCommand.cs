using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using System;

namespace OpenTibia.Game.Commands
{
    public class LogOutCommand : Command
    {
        public LogOutCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        public override Promise Execute(Context context)
        {
            Tile fromTile = Player.Tile;

            return context.AddCommand(new PlayerDestroyCommand(Player) ).Then(ctx =>
            {
                return ctx.AddCommand(new ShowMagicEffectCommand(fromTile.Position, MagicEffectType.Puff) );

            } ).Then(ctx =>
            {
                ctx.Disconnect(Player.Client.Connection);
            } );
        }
    }
}