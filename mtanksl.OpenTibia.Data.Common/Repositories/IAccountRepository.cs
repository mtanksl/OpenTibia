using OpenTibia.Data.Models;
using System.Threading.Tasks;

namespace OpenTibia.Data.Repositories
{
    public interface IAccountRepository
    {
        Task<DbAccount> GetAccount(string accountName, string accountPassword);

        Task<DbAccount> GetAccountById(int id);

        Task<DbAccount> GetAccountByName(string name);

        void AddDbAccount(DbAccount account);
    }
}