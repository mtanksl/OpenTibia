using OpenTibia.Common.Objects;
using OpenTibia.Data.Models;

namespace OpenTibia.Game.Commands
{
    public class TileCreatePlayerCommand : CommandResult<Player>
    {
        public TileCreatePlayerCommand(Tile tile, IConnection connection, DbPlayer dbPlayer)
        {
            Tile = tile;

            Connection = connection;

            DbPlayer = dbPlayer;
        }

        public Tile Tile { get; set; }

        public IConnection Connection { get; set; }

        public DbPlayer DbPlayer { get; set; }

        public override PromiseResult<Player> Execute()
        {
            Player player = Context.Server.PlayerFactory.Create(Connection, DbPlayer, Tile);

            if (player != null)
            {
                foreach (var dbPlayerStorage in DbPlayer.PlayerStorages)
                {
                    player.Client.Storages.SetValue(dbPlayerStorage.Key, dbPlayerStorage.Value);
                }

                foreach (var dbVip in DbPlayer.PlayerVips)
                {
                    player.Client.Vips.AddVip(dbVip.Vip.Id, dbVip.Vip.Name);
                }

                return Context.AddCommand(new TileAddCreatureCommand(Tile, player) ).Then( () =>
                {
                    return Promise.FromResult(player); 
                } );
            }

            return Promise.FromResult(player);
        }           
    }
}