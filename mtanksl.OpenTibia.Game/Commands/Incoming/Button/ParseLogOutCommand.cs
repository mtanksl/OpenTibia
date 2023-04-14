using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class ParseLogOutCommand : Command
    {
        public ParseLogOutCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        public override Promise Execute()
        {
            Tile fromTile = Player.Tile;

            return Context.AddCommand(new ShowMagicEffectCommand(fromTile.Position, MagicEffectType.Puff) ).Then( () =>
            {
                return Context.AddCommand(new PlayerDestroyCommand(Player) );
            } );
        }
    }
}