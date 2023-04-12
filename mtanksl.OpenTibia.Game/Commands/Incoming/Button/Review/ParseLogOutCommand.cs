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
            return Promise.Run( (resolve, reject) =>
            {
                Tile fromTile = Player.Tile;

                if (fromTile != null)
                {
                    Context.AddCommand(new ShowMagicEffectCommand(fromTile.Position, MagicEffectType.Puff) );

                    Context.AddCommand(new PlayerDestroyCommand(Player) );
                }

                Context.Disconnect(Player.Client.Connection);

                resolve();
            } );
        }
    }
}