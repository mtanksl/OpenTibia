using Microsoft.EntityFrameworkCore;
using OpenTibia.Data.Contexts;
using OpenTibia.Data.Models;
using System.Linq;
using System.Threading.Tasks;

namespace OpenTibia.Data.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private DatabaseContext context;

        public PlayerRepository(DatabaseContext context)
        {
            this.context = context;
        }

        public async Task<DbPlayer> GetPlayer(string accountName, string accountPassword, string playerName)
        {
            await Task.Yield();

            DbPlayer player = await context.Players
                .Include(p => p.Account)
                .Where(p => p.Account.Name == accountName &&
                            p.Account.Password == accountPassword &&
                            p.Name == playerName)
                .FirstOrDefaultAsync();

            if (player != null)
            {
                await context.PlayerDepotItems
                    .Where(pi => pi.PlayerId == player.Id)
                    .LoadAsync();

                await context.PlayerItems
                    .Where(pi => pi.PlayerId == player.Id)
                    .LoadAsync();

                await context.PlayerStorages
                    .Where(pi => pi.PlayerId == player.Id)
                    .LoadAsync();

                await context.PlayerSpells
                    .Where(pi => pi.PlayerId == player.Id)
                    .LoadAsync();

                await context.PlayerBlesses
                    .Where(pi => pi.PlayerId == player.Id)
                    .LoadAsync();

                await context.PlayerAchievements
                    .Where(pi => pi.PlayerId == player.Id)
                    .LoadAsync();

                await context.PlayerOutfits
                    .Where(pi => pi.PlayerId == player.Id)
                    .LoadAsync();

                await context.PlayerVips
                    .Include(v => v.Vip)
                    .Where(pi => pi.PlayerId == player.Id)
                    .LoadAsync();
            }

            return player;
        }

        public async Task<DbPlayer[]> GetPlayerByIds(int[] ids)
        {
            await Task.Yield();

            DbPlayer[] players = await context.Players
                .Where(p => ids.Contains(p.Id) )
                .ToArrayAsync();

            if (players.Length > 0)
            {
                await context.PlayerDepotItems
                    .Where(pi => ids.Contains(pi.PlayerId) )
                    .LoadAsync();

                await context.PlayerItems
                    .Where(pi => ids.Contains(pi.PlayerId) )
                    .LoadAsync();

                await context.PlayerStorages
                    .Where(pi => ids.Contains(pi.PlayerId) )
                    .LoadAsync();

                await context.PlayerSpells
                    .Where(pi => ids.Contains(pi.PlayerId) )
                    .LoadAsync();

                await context.PlayerBlesses
                    .Where(pi => ids.Contains(pi.PlayerId) )
                    .LoadAsync();

                await context.PlayerAchievements
                    .Where(pi => ids.Contains(pi.PlayerId) )
                    .LoadAsync();

                await context.PlayerOutfits
                    .Where(pi => ids.Contains(pi.PlayerId) )
                    .LoadAsync();

                await context.PlayerVips
                    .Include(v => v.Vip)
                    .Where(pi => ids.Contains(pi.PlayerId) )
                    .LoadAsync();              
            }

            return players;
        }

        public async Task<DbPlayer> GetPlayerByName(string name)
        {
            await Task.Yield();

            DbPlayer player = await context.Players
                .Where(p => p.Name == name)
                .FirstOrDefaultAsync();

            if (player != null)
            {
                await context.PlayerOutfits
                    .Where(pi => pi.PlayerId == player.Id)
                    .LoadAsync();
            }

            return player;
        }

        public void AddPlayer(DbPlayer player)
        {
            context.Players.Add(player);
        }
    }
}