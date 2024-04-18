using OpenTibia.Data.Models;

namespace OpenTibia.Data.Repositories
{
    public interface IHouseRepository
    {
        DbHouse[] GetHouses();

        void AddHouse(DbHouse house);
    }
}