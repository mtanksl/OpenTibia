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

            return Context.AddCommand(new TileAddCreatureCommand(Tile, player) ).Then( () =>
            {
                Context.AddEvent(new PlayerLoginEventArgs(player) );

                return Promise.FromResult(player); 
            } );
        }        
    }
}