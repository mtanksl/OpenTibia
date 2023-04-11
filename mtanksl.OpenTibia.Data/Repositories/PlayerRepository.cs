using Microsoft.EntityFrameworkCore;
using OpenTibia.Data.Contexts;
using OpenTibia.Data.Models;
using System.Linq;

namespace OpenTibia.Data.Repositories
{
    public class PlayerRepository
    {
        private SqliteContext context;

        public PlayerRepository(SqliteContext context)
        {
            this.context = context;
        }

        public Account GetAccount(string accountName, string accountPassword)
        {
            return context.Accounts
                .Include(a => a.Players)
                    .ThenInclude(p => p.World)
                .Where(a => a.Name == accountName &&
                            a.Password == accountPassword)
                .FirstOrDefault();
        }

        public Player GetAccountPlayer(string accountName, string accountPassword, string playerName)
        {
            return context.Players
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
            return context.Players
                .Include(p => p.PlayerItems)
                .Include(p => p.PlayerDepotItems)
                .Include(p => p.PlayerVips)
                .Where(p => p.Id == databasePlayerId)
                .FirstOrDefault();
        }

        public Player GetPlayerByName(string name)
        {
            return context.Players
                .Where(p => p.Name == name)
                .FirstOrDefault();
        }

        public void UpdatePlayer(Player player)
        {
            context.Entry(player).State = EntityState.Modified;
        }

        public void AddPlayerItem(PlayerItem playerItem)
        {
            context.PlayerItems.Add(playerItem);
        }

        public void RemovePlayerItem(PlayerItem playerItem)
        {
            context.PlayerItems.Remove(playerItem);
        }

        public void AddPlayerDepotItem(PlayerDepotItem playerDepotItem)
        {
            context.PlayerDepotItems.Add(playerDepotItem);
        }

        public void RemovePlayerDepotItem(PlayerDepotItem playerDepotItem)
        {
            context.PlayerDepotItems.Remove(playerDepotItem);
        }

        public void AddPlayerVip(PlayerVip playerVip)
        {
            context.PlayerVips.Add(playerVip);
        }

        public void RemovePlayerVip(PlayerVip playerVip)
        {
            context.PlayerVips.Remove(playerVip);
        }
    }
}