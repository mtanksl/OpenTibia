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

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                Tile fromTile = Player.Tile;

                if (fromTile != null)
                {
                    context.AddCommand(new ShowMagicEffectCommand(fromTile.Position, MagicEffectType.Puff) );

                    context.AddCommand(new PlayerDestroyCommand(Player) );
                }

                context.Disconnect(Player.Client.Connection);

                resolve(context);
            } );
        }
    }
}