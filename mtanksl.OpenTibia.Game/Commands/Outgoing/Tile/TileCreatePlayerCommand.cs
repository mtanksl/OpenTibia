using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class TileCreatePlayerCommand : CommandResult<Player>
    {
        public TileCreatePlayerCommand(Tile tile, Data.Models.Player databasePlayer)
        {
            Tile = tile;

            DatabasePlayer = databasePlayer;
        }

        public Tile Tile { get; set; }

        public Data.Models.Player DatabasePlayer { get; set; }

        public override PromiseResult<Player> Execute(Context context)
        {
            return PromiseResult<Player>.Run(resolve =>
            {
                Player player = context.Server.PlayerFactory.Create();

                player.DatabasePlayerId = DatabasePlayer.Id;

                player.Name = DatabasePlayer.Name;
#if DEBUG
                player.BaseSpeed = player.Speed = 2218;
#endif
                //TODO: Load inventory from database

                if (player != null)
                {
                    context.AddCommand(new TileAddCreatureCommand(Tile, player) ).Then( (ctx, index) =>
                    {
                        resolve(ctx, player);
                    } );
                }
                else
                {
                    resolve(context, player);
                }
            } );
        }
    }
}