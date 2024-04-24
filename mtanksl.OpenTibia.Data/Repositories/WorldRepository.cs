using OpenTibia.Data.Contexts;
using OpenTibia.Data.Models;
using System.Linq;

namespace OpenTibia.Data.Repositories
{
    public class WorldRepository : IWorldRepository
    {
        private DatabaseContext context;

        public WorldRepository(DatabaseContext context)
        {
            this.context = context;
        }

        public DbWorld[] GetWorlds()
        {
            return context.Worlds
                .ToArray();
        }
    }
}