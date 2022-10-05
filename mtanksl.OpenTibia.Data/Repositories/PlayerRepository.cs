using OpenTibia.Data.Contexts;
using OpenTibia.Data.Models;
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
                .Where(p => p.Account.Name == accountName &&
                            p.Account.Password == accountPassword &&
                            p.Name == playerName)
                .FirstOrDefault();
        }
    }
}