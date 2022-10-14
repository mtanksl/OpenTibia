using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class TileCreatePlayerCommand : CommandResult<Player>
    {
        public TileCreatePlayerCommand(Tile tile, string name)
        {
            Tile = tile;

            Name = name;
        }

        public Tile Tile { get; set; }

        public string Name { get; set; }

        public override PromiseResult<Player> Execute(Context context)
        {
            return PromiseResult<Player>.Run(resolve =>
            {
                Player player = context.Server.PlayerFactory.Create(Name);

                if (player != null)
                {
                    context.AddCommand(new TileAddCreatureCommand(Tile, player) ).Then( (ctx, index) =>
                    {
                        resolve(ctx, player);
                    } );
                }
            } );
        }
    }
}