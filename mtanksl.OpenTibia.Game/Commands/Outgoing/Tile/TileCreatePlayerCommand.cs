using OpenTibia.Common.Objects;

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
            Client client = new Client(Context.Server);

            client.Connection = Connection;

            Player player = Context.Server.PlayerFactory.Create(DatabasePlayer);

            player.Client = client;

            Context.Server.Logger.WriteLine(player.Name + " connected.", LogLevel.Information);

            //TODO: Load from database

            return Context.AddCommand(new TileAddCreatureCommand(Tile, player) ).Then( () =>
            {
                return Promise.FromResult(player); 
            } );
        }
    }
}