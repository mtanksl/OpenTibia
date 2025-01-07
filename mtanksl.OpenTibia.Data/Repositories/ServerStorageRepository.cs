using Microsoft.EntityFrameworkCore;
using OpenTibia.Data.Contexts;
using OpenTibia.Data.Models;
using System.Linq;
using System.Threading.Tasks;

namespace OpenTibia.Data.Repositories
{
    public class ServerStorageRepository : IServerStorageRepository
    {
        private DatabaseContext context;

        public ServerStorageRepository(DatabaseContext context)
        {
            this.context = context;
        }

        public async Task<DbServerStorage[]> GetServerStorages()
        {
            await Task.Yield();

            return await context.ServerStorages
                .ToArrayAsync();
        }

        public async Task<DbServerStorage> GetServerStorageByKey(string key)
        {
            await Task.Yield();

            return await context.ServerStorages
                .Where(m => m.Key == key)
                .FirstOrDefaultAsync();
        }

        public void AddServerStorage(DbServerStorage serverStorage)
        {
            context.ServerStorages.Add(serverStorage);
        }
    }
}