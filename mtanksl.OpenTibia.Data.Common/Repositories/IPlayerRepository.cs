using OpenTibia.Data.Models;
using System.Threading.Tasks;

namespace OpenTibia.Data.Repositories
{
    public interface IPlayerRepository
    {
        Task<DbPlayer> GetPlayer(string accountName, string accountPassword, string playerName);

        Task<DbPlayer[]> GetPlayerByIds(int[] ids);

        Task<DbPlayer> GetPlayerByName(string name);

        void AddPlayer(DbPlayer player);
    }
}