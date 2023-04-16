using OpenTibia.Common.Objects;
using OpenTibia.Game.Events;

namespace OpenTibia.Game.Commands
{
    public class TileCreatePlayerCommand : CommandResult<Player>
    {
        public TileCreatePlayerCommand(Tile tile, IConnection connection, Data.Models.Player databasePlayer)
        {
            Tile = tile;

            Connection = connection;

            DatabasePlayer = databasePlayer;
        }

        public Tile Tile { get; set; }

        public IConnection Connection { get; set; }

        public Data.Models.Player DatabasePlayer { get; set; }

        public override PromiseResult<Player> Execute()
        {
            Player player = Context.Server.PlayerFactory.Create(Connection, DatabasePlayer);

            Context.AddEvent(new PlayerLoginEventArgs(player) );

            return Context.AddCommand(new TileAddCreatureCommand(Tile, player) ).Then( () =>
            {
                return Promise.FromResult(player); 
            } );
        }
    }
}