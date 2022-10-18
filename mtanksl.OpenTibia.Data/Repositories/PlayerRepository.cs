using OpenTibia.Data.Contexts;
using OpenTibia.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
                .Include(a => a.Players.Select(p => p.World) )
                .Where(a => a.Name == accountName &&
                            a.Password == accountPassword)
                .FirstOrDefault();
        }

        public Player GetPlayer(string accountName, string accountPassword, string playerName)
        {
            return context.Players
                .Include(p => p.PlayerItems)
                .Include(p => p.PlayerDepotItems)
                .Where(p => p.Account.Name == accountName &&
                            p.Account.Password == accountPassword &&
                            p.Name == playerName)
                .FirstOrDefault();
        }

        public Player GetPlayer(int databasePlayerId)
        {
            return context.Players
                .Include(p => p.PlayerItems)
                .Include(p => p.PlayerDepotItems)
                .Where(p => p.Id == databasePlayerId)
                .FirstOrDefault();
        }

        public void UpdatePlayer(Player player)
        {

        }

        public List<PlayerItem> GetPlayerItems(int databasePlayerId)
        {
            return context.PlayerItems
                .Where(p => p.PlayerId == databasePlayerId)
                .ToList();
        }

        public void AddPlayerItem(PlayerItem playerItem)
        {
            context.PlayerItems.Add(playerItem);
        }

        public void RemovePlayerItem(PlayerItem playerItem)
        {
            context.PlayerItems.Remove(playerItem);
        }

        public List<PlayerDepotItem> GetPlayerDepotItems(int databasePlayerId)
        {
            return context.PlayerDepotItems
                .Where(p => p.PlayerId == databasePlayerId)
                .ToList();
        }

        public void AddPlayerDepotItem(PlayerDepotItem playerDepotItem)
        {
            context.PlayerDepotItems.Add(playerDepotItem);
        }

        public void RemovePlayerDepotItem(PlayerDepotItem playerDepotItem)
        {
            context.PlayerDepotItems.Remove(playerDepotItem);
        }
    }
}