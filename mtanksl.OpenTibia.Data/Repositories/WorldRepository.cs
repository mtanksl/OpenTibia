using Microsoft.EntityFrameworkCore;
using OpenTibia.Data.Contexts;
using OpenTibia.Data.Models;
using System.Linq;
using System.Threading.Tasks;

namespace OpenTibia.Data.Repositories
{
    public class WorldRepository : IWorldRepository
    {
        private DatabaseContext context;

        public WorldRepository(DatabaseContext context)
        {
            this.context = context;
        }

        public async Task<DbWorld[]> GetWorlds()
        {
            await Task.Yield();

            return await context.Worlds
                .ToArrayAsync();
        }

        public async Task<DbWorld> GetWorldByName(string name)
        {
            await Task.Yield();

            return await context.Worlds
                .Where(w => w.Name == name)
                .FirstOrDefaultAsync();
        }
    }
}