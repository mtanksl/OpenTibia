using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Data.Models;

namespace OpenTibia.Game.Commands
{
    public class TileCreatePlayerCommand : CommandResult<Player>
    {
        public TileCreatePlayerCommand(IConnection connection, DbPlayer dbPlayer)
        {
            Connection = connection;

            DbPlayer = dbPlayer;
        }

        public IConnection Connection { get; set; }

        public DbPlayer DbPlayer { get; set; }

        public override PromiseResult<Player> Execute()
        {
            Client client = new Client(Context.Server);

            client.Connection = Connection;

            Player player = Context.Server.GameObjectPool.GetPlayer(DbPlayer.Name);

            if (player == null)
            {
                Tile spawn = Context.Server.Map.GetTile(new Position(DbPlayer.SpawnX, DbPlayer.SpawnY, DbPlayer.SpawnZ));

                if (spawn == null)
                {
                    spawn = Context.Server.Map.GetTile(new Position(DbPlayer.TownX, DbPlayer.TownY, DbPlayer.TownZ));
                }

                player = Context.Server.PlayerFactory.Create(DbPlayer.Id, DbPlayer.Name, spawn);

                player.Client = client;

                Context.Server.PlayerFactory.Load(DbPlayer, player);

                Context.Server.GameObjectPool.AddPlayer(player);
            }
            else
            {
                player.Client = client;
            }

            Context.Server.PlayerFactory.Attach(player);

            return Context.AddCommand(new TileAddCreatureCommand(player.Spawn, player) ).Then( () =>
            {
                return Context.AddCommand(new PlayerLoginCommand(player) );     
                    
            } ).Then( () =>
            {
                return Promise.FromResult(player); 
            } );
        }
    }
}