using OpenTibia.Common.Objects;
using OpenTibia.Data.Models;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class TileCreatePlayerCommand : CommandResult<Player>
    {
        public TileCreatePlayerCommand(Tile tile, IConnection connection, DbPlayer databasePlayer)
        {
            Tile = tile;

            Connection = connection;

            DatabasePlayer = databasePlayer;
        }

        public Tile Tile { get; set; }

        public IConnection Connection { get; set; }

        public DbPlayer DatabasePlayer { get; set; }

        public override PromiseResult<Player> Execute()
        {
            Player player = Context.Server.PlayerFactory.Create(Connection, DatabasePlayer, Tile);

            #region TODO: Review

            LoadInventory(Context, player, DatabasePlayer);

            LoadLocker(Context, DatabasePlayer);

            LoadVip(player, DatabasePlayer);

            #endregion

            return Context.AddCommand(new TileAddCreatureCommand(Tile, player) ).Then( () =>
            {
                return Promise.FromResult(player); 
            } );
        }    
        
        private static void LoadInventory(Context context, Player player, DbPlayer databasePlayer)
        {
            void AddItems(Container container, int sequenceId)
            {
                foreach (var playerItem in databasePlayer.PlayerItems.Where(i => i.ParentId == sequenceId) )
                {
                    var item = context.Server.ItemFactory.Create( (ushort)playerItem.OpenTibiaId, (byte)playerItem.Count);

                    if (item is Container container2)
                    {
                        AddItems(container2, playerItem.SequenceId);
                    }

                    container.AddContent(item);
                }
            }

            foreach (var playerItem in databasePlayer.PlayerItems.Where(i => i.ParentId >= 1 /* Slot.Head */ && i.ParentId <= 10 /* Slot.Extra */ ) )
            {
                var item = context.Server.ItemFactory.Create( (ushort)playerItem.OpenTibiaId, (byte)playerItem.Count);

                if (item is Container container)
                {
                    AddItems(container, playerItem.SequenceId);
                }

                player.Inventory.AddContent(item, (byte)playerItem.ParentId);
            }
        }

        private static void LoadLocker(Context context, DbPlayer databasePlayer)
        {
            void AddItems(Container container, int sequenceId)
            {
                foreach (var playerDepotItem in databasePlayer.PlayerDepotItems.Where(i => i.ParentId == sequenceId) )
                {
                    var item = context.Server.ItemFactory.Create( (ushort)playerDepotItem.OpenTibiaId, (byte)playerDepotItem.Count);

                    if (item is Container container2)
                    {
                        AddItems(container2, playerDepotItem.SequenceId);
                    }

                    container.AddContent(item);
                }
            }

            foreach (var playerDepotItem in databasePlayer.PlayerDepotItems.Where(i => i.ParentId >= 0 /* Town Id */ && i.ParentId <= 100 /* Town Id */ ) )
            {
                var container = (Container)context.Server.ItemFactory.Create(2591, 1);

                AddItems(container, playerDepotItem.SequenceId);

                context.Server.Lockers.AddLocker(databasePlayer.Id, (ushort)playerDepotItem.ParentId, container);
            }
        }

        private static void LoadVip(Player player, DbPlayer databasePlayer)
        {
            foreach (var playerVip in databasePlayer.PlayerVips)
            {
                player.Client.Vips.AddVip(playerVip.Vip.Name);
            }
        }
    }
}