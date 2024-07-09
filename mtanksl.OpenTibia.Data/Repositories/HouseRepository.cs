using Microsoft.EntityFrameworkCore;
using OpenTibia.Data.Contexts;
using OpenTibia.Data.Models;
using System.Linq;

namespace OpenTibia.Data.Repositories
{
    public class HouseRepository : IHouseRepository
    {
        private DatabaseContext context;

        public HouseRepository(DatabaseContext context)
        {
            this.context = context;
        }

        public DbHouse[] GetHouses()
        {
            DbHouse[] houses = context.Houses
                .Include(h => h.Owner)
                .ToArray();

            if (houses.Length > 0)
            {
                context.HouseAccessLists
                    .Load();

                context.HouseItems
                    .Load();
            }

            return houses;
        }

        public void AddHouse(DbHouse house)
        {
            context.Houses.Add(house);
        }
    }
}