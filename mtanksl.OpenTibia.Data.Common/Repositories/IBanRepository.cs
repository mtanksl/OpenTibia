using OpenTibia.Data.Models;

namespace OpenTibia.Data.Repositories
{
    public interface IBanRepository
    {
        DbBan GetBanByAccountId(int accountId);

        DbBan GetBanByPlayerId(int playerId);
        
        DbBan GetBanByIpAddress(string ipAddress);
        
        void AddBan(DbBan ban);
        
        void RemoveBan(DbBan ban);
    }
}