using OpenTibia.Data.Contexts;
using OpenTibia.Data.Models;
using System.Linq;

namespace OpenTibia.Data.Repositories
{
    public class BanRepository
    {
        private SqliteContext sqliteContext;

        public BanRepository(SqliteContext sqliteContext)
        {
            this.sqliteContext = sqliteContext;
        }

        public DbBan GetBanByAccountId(int accountId)
        {
            return sqliteContext.Bans
                .Where(p => p.AccountId == accountId)
                .FirstOrDefault();
        }

        public DbBan GetBanByPlayerId(int playerId)
        {
            return sqliteContext.Bans
                .Where(p => p.PlayerId == playerId)
                .FirstOrDefault();
        }

        public DbBan GetBanByIpAddress(string ipAddress)
        {
            return sqliteContext.Bans
                .Where(p => p.IpAddress == ipAddress)
                .FirstOrDefault();
        }

        public void AddBan(DbBan ban)
        {
            sqliteContext.Bans.Add(ban);
        }

        public void RemoveBan(DbBan ban)
        {
            sqliteContext.Bans.Remove(ban);
        }
    }
}