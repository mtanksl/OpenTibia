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
            Account account = sqliteContext.Accounts
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

        public Player GetAccountPlayer(string accountName, string accountPassword, string playerName)
        {
            Player player = sqliteContext.Players
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

        public Player GetPlayerById(int databasePlayerId)
        {
            Player player = sqliteContext.Players
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