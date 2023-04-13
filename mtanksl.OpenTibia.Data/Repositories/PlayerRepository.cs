using Microsoft.EntityFrameworkCore;
using OpenTibia.Data.Contexts;
using OpenTibia.Data.Models;
using System.Linq;

namespace OpenTibia.Data.Repositories
{
    public class PlayerRepository
    {
        private SqliteContext sqliteContext;

        public PlayerRepository(SqliteContext sqliteContext)
        {
            this.sqliteContext = sqliteContext;
        }

        public Account GetAccount(string accountName, string accountPassword)
        {
            return sqliteContext.Accounts
                .Include(a => a.Players)
                    .ThenInclude(p => p.World)
                .Where(a => a.Name == accountName &&
                            a.Password == accountPassword)
                .FirstOrDefault();
        }

        public Player GetAccountPlayer(string accountName, string accountPassword, string playerName)
        {
            return sqliteContext.Players
                .Include(p => p.PlayerItems)
                .Include(p => p.PlayerDepotItems)
                .Include(p => p.PlayerVips)
                    .ThenInclude(v => v.Vip)
                .Where(p => p.Account.Name == accountName &&
                            p.Account.Password == accountPassword &&
                            p.Name == playerName)
                .FirstOrDefault();
        }

        public Player GetPlayerById(int databasePlayerId)
        {
            return sqliteContext.Players
                .Include(p => p.PlayerItems)
                .Include(p => p.PlayerDepotItems)
                .Include(p => p.PlayerVips)
                .Where(p => p.Id == databasePlayerId)
                .FirstOrDefault();
        }

        public Player GetPlayerByName(string name)
        {
            return sqliteContext.Players
                .Where(p => p.Name == name)
                .FirstOrDefault();
        }

        public void UpdatePlayer(Player player)
        {
            sqliteContext.Entry(player).State = EntityState.Modified;
        }

        public void AddPlayerItem(PlayerItem playerItem)
        {
            sqliteContext.PlayerItems.Add(playerItem);
        }

        public void RemovePlayerItem(PlayerItem playerItem)
        {
            sqliteContext.PlayerItems.Remove(playerItem);
        }

        public void AddPlayerDepotItem(PlayerDepotItem playerDepotItem)
        {
            sqliteContext.PlayerDepotItems.Add(playerDepotItem);
        }

        public void RemovePlayerDepotItem(PlayerDepotItem playerDepotItem)
        {
            sqliteContext.PlayerDepotItems.Remove(playerDepotItem);
        }

        public void AddPlayerVip(PlayerVip playerVip)
        {
            sqliteContext.PlayerVips.Add(playerVip);
        }

        public void RemovePlayerVip(PlayerVip playerVip)
        {
            sqliteContext.PlayerVips.Remove(playerVip);
        }
    }
}