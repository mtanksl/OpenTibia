using OpenTibia.Data.Contexts;
using OpenTibia.Data.Models;
using System.Linq;

namespace OpenTibia.Data.Repositories
{
    public class BanRepository : IBanRepository
    {
        private DatabaseContext context;

        public BanRepository(DatabaseContext context)
        {
            this.context = context;
        }

        public DbBan GetBanByAccountId(int accountId)
        {
            return context.Bans
                .Where(b => b.AccountId == accountId)
                .FirstOrDefault();
        }

        public DbBan GetBanByPlayerId(int playerId)
        {
            return context.Bans
                .Where(b => b.PlayerId == playerId)
                .FirstOrDefault();
        }

        public DbBan GetBanByIpAddress(string ipAddress)
        {
            return context.Bans
                .Where(b => b.IpAddress == ipAddress)
                .FirstOrDefault();
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