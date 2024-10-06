using Microsoft.EntityFrameworkCore;
using OpenTibia.Data.Contexts;
using OpenTibia.Data.Models;
using System.Threading.Tasks;

namespace OpenTibia.Data.Repositories
{
    public class HouseRepository : IHouseRepository
    {
        private DatabaseContext context;

        public HouseRepository(DatabaseContext context)
        {
            this.context = context;
        }

        public async Task<DbHouse[]> GetHouses()
        {
            await Task.Yield();

            DbHouse[] houses = await context.Houses
                .Include(h => h.Owner)
                .ToArrayAsync();

            if (houses.Length > 0)
            {
                await context.HouseAccessLists
                    .LoadAsync();

                await context.HouseItems
                    .LoadAsync();
            }

            return houses;
        }

        public void AddHouse(DbHouse house)
        {
            context.Houses.Add(house);
        }
    }
}