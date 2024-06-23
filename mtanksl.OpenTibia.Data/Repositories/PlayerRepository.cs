using Microsoft.EntityFrameworkCore;
using OpenTibia.Data.Contexts;
using OpenTibia.Data.Models;
using System.Linq;

namespace OpenTibia.Data.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private DatabaseContext context;

        public PlayerRepository(DatabaseContext context)
        {
            this.context = context;
        }

        public DbAccount GetAccount(string accountName, string accountPassword)
        {
            DbAccount account = context.Accounts
                .Where(a => a.Name == accountName &&
                            a.Password == accountPassword)
                .FirstOrDefault();

            if (account != null)
            {
                context.Players
                    .Include(p => p.World)
                    .Where(p => p.AccountId == account.Id)
                    .Load();
            }

            return account;
        }

        public DbPlayer GetAccountPlayer(string accountName, string accountPassword, string playerName)
        {
            DbPlayer player = context.Players
                .Where(p => p.Account.Name == accountName &&
                            p.Account.Password == accountPassword &&
                            p.Name == playerName)
                .FirstOrDefault();

            if (player != null)
            {
                context.PlayerDepotItems
                    .Where(pi => pi.PlayerId == player.Id)
                    .Load();

                context.PlayerItems
                    .Where(pi => pi.PlayerId == player.Id)
                    .Load();

                context.PlayerStorages
                    .Where(pi => pi.PlayerId == player.Id)
                    .Load();

                context.PlayerSpells
                    .Where(pi => pi.PlayerId == player.Id)
                    .Load();

                context.PlayerBlesses
                   .Where(pi => pi.PlayerId == player.Id)
                   .Load();

                context.PlayerAchievements
                   .Where(pi => pi.PlayerId == player.Id)
                   .Load();

                context.PlayerOutfits
                    .Where(pi => pi.PlayerId == player.Id)
                    .Load();

                context.PlayerVips
                    .Include(v => v.Vip)
                    .Where(pi => pi.PlayerId == player.Id)
                    .Load();
            }

            return player;
        }

        public DbPlayer[] GetPlayerByIds(int[] ids)
        {
            DbPlayer[] players = context.Players
                .Where(p => ids.Contains(p.Id) )
                .ToArray();

            if (players.Length > 0)
            {
                context.PlayerDepotItems
                    .Where(pi => ids.Contains(pi.PlayerId) )
                    .Load();

                context.PlayerItems
                    .Where(pi => ids.Contains(pi.PlayerId) )
                    .Load();

                context.PlayerStorages
                    .Where(pi => ids.Contains(pi.PlayerId) )
                    .Load();

                context.PlayerSpells
                    .Where(pi => ids.Contains(pi.PlayerId) )
                    .Load(); 
                
                context.PlayerBlesses
                    .Where(pi => ids.Contains(pi.PlayerId) )
                    .Load();

                context.PlayerAchievements
                    .Where(pi => ids.Contains(pi.PlayerId) )
                    .Load();

                context.PlayerOutfits
                    .Where(pi => ids.Contains(pi.PlayerId) )
                    .Load();

                context.PlayerVips
                    .Include(v => v.Vip)
                    .Where(pi => ids.Contains(pi.PlayerId) )
                    .Load();              
            }

            return players;
        }

        public DbPlayer GetPlayerByName(string name)
        {
            return context.Players
                .Where(p => p.Name == name)
                .FirstOrDefault();
        }
    }
}