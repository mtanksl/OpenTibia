using Microsoft.EntityFrameworkCore;
using OpenTibia.Data.Contexts;
using OpenTibia.Data.Models;
using System.Linq;
using System.Threading.Tasks;

namespace OpenTibia.Data.Repositories
{
    public class BanRepository : IBanRepository
    {
        private DatabaseContext context;

        public BanRepository(DatabaseContext context)
        {
            this.context = context;
        }

        public async Task<DbBan> GetBanByAccountId(int accountId)
        {
            await Task.Yield();

            return await context.Bans
                .Where(b => b.AccountId == accountId)
                .FirstOrDefaultAsync();
        }

        public async Task<DbBan> GetBanByPlayerId(int playerId)
        {
            await Task.Yield();

            return await context.Bans
                .Where(b => b.PlayerId == playerId)
                .FirstOrDefaultAsync();
        }

        public async Task<DbBan> GetBanByIpAddress(string ipAddress)
        {
            await Task.Yield();

            return await context.Bans
                .Where(b => b.IpAddress == ipAddress)
                .FirstOrDefaultAsync();
        }

        public void AddBan(DbBan ban)
        {
            context.Bans.Add(ban);
        }

        public void RemoveBan(DbBan ban)
        {
            context.Bans.Remove(ban);
        }
    }
}