using OpenTibia.Data.Models;
using System.Threading.Tasks;

namespace OpenTibia.Data.Repositories
{
    public interface IHouseRepository
    {
        Task<DbHouse[]> GetHouses();

        void AddHouse(DbHouse house);
    }
}