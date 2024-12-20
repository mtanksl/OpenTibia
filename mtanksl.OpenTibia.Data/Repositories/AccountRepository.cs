using Microsoft.EntityFrameworkCore;
using OpenTibia.Data.Contexts;
using OpenTibia.Data.Models;
using System.Linq;
using System.Threading.Tasks;

namespace OpenTibia.Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private DatabaseContext context;

        public AccountRepository(DatabaseContext context)
        {
            this.context = context;
        }

        public async Task<DbAccount> GetAccount(string accountName, string accountPassword)
        {
            await Task.Yield();
            
            DbAccount account = await context.Accounts
                .Where(a => a.Name == accountName &&
                            a.Password == accountPassword)
                .FirstOrDefaultAsync();

            if (account != null)
            {
                await context.Players
                    .Include(p => p.World)
                    .Where(p => p.AccountId == account.Id)
                    .LoadAsync();
            }

            return account;
        }

        public async Task<DbAccount> GetAccountByName(string name)
        {
            await Task.Yield();

            return await context.Accounts
                .Where(p => p.Name == name)
                .FirstOrDefaultAsync();
        }

        public void AddDbAccount(DbAccount account)
        {
            context.Accounts.Add(account);
        }
    }
}