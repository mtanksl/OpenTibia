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

        public DbAccount GetAccount(string accountName, string accountPassword)
        {
            DbAccount account = sqliteContext.Accounts
                .Where(a => a.Name == accountName &&
                            a.Password == accountPassword)
                .FirstOrDefault();

            if (account != null)
            {
                sqliteContext.Players
                    .Include(p => p.World)
                    .Where(p => p.AccountId == account.Id)
                    .Load();
            }

            return account;
        }

        public DbPlayer GetAccountPlayer(string accountName, string accountPassword, string playerName)
        {
            DbPlayer player = sqliteContext.Players
                .Where(p => p.Account.Name == accountName &&
                            p.Account.Password == accountPassword &&
                            p.Name == playerName)
                .FirstOrDefault();

            if (player != null)
            {
                sqliteContext.PlayerItems
                    .Where(pi => pi.PlayerId == player.Id)
                    .Load();

                sqliteContext.PlayerDepotItems
                    .Where(pi => pi.PlayerId == player.Id)
                    .Load();

                sqliteContext.PlayerVips
                    .Include(v => v.Vip)
                    .Where(pi => pi.PlayerId == player.Id)
                    .Load();
            }

            return player;
        }

        public DbPlayer GetPlayerById(int databasePlayerId)
        {
            DbPlayer player = sqliteContext.Players
                .Where(p => p.Id == databasePlayerId)
                .FirstOrDefault();

            if (player != null)
            {
                sqliteContext.PlayerItems
                    .Where(pi => pi.PlayerId == player.Id)
                    .Load();

                sqliteContext.PlayerDepotItems
                    .Where(pi => pi.PlayerId == player.Id)
                    .Load();

                sqliteContext.PlayerVips
                    .Include(v => v.Vip)
                    .Where(pi => pi.PlayerId == player.Id)
                    .Load();
            }

            return player;
        }

        public DbPlayer GetPlayerByName(string name)
        {
            return sqliteContext.Players
                .Where(p => p.Name == name)
                .FirstOrDefault();
        }

        public void UpdatePlayer(DbPlayer player)
        {
            sqliteContext.Entry(player).State = EntityState.Modified;
        }

        public void AddPlayerItem(DbPlayerItem playerItem)
        {
            sqliteContext.PlayerItems.Add(playerItem);
        }

        public void RemovePlayerItem(DbPlayerItem playerItem)
        {
            sqliteContext.PlayerItems.Remove(playerItem);
        }

        public void AddPlayerDepotItem(DbPlayerDepotItem playerDepotItem)
        {
            sqliteContext.PlayerDepotItems.Add(playerDepotItem);
        }

        public void RemovePlayerDepotItem(DbPlayerDepotItem playerDepotItem)
        {
            sqliteContext.PlayerDepotItems.Remove(playerDepotItem);
        }

        public void AddPlayerVip(DbPlayerVip playerVip)
        {
            sqliteContext.PlayerVips.Add(playerVip);
        }

        public void RemovePlayerVip(DbPlayerVip playerVip)
        {
            sqliteContext.PlayerVips.Remove(playerVip);
        }
    }
}