using OpenTibia.Data.Models;
using System.Threading.Tasks;

namespace OpenTibia.Data.Repositories
{
    public interface IBanRepository
    {
        Task<DbBan> GetBanByAccountId(int accountId);

        Task<DbBan> GetBanByPlayerId(int playerId);

        Task<DbBan> GetBanByIpAddress(string ipAddress);
        
        void AddBan(DbBan ban);
        
        void RemoveBan(DbBan ban);
    }
}