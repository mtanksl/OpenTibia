using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class PlayerCreateCommand : CommandResult<Player>
    {
        public PlayerCreateCommand(Tile tile, string name)
        {
            Tile = tile;

            Name = name;
        }

        public Tile Tile { get; set; }

        public string Name { get; set; }

        public override void Execute(Context context)
        {
            Player player = context.Server.PlayerFactory.Create(Name);

            if (player != null)
            {
                context.AddCommand(new TileAddCreatureCommand(Tile, player), ctx =>
                {
                    OnComplete(ctx, player);
                } );
            }
        }
    }
}