using Microsoft.EntityFrameworkCore;
using OpenTibia.Data.Contexts;
using OpenTibia.Data.Models;
using System.Linq;
using System.Threading.Tasks;

namespace OpenTibia.Data.Repositories
{
    public class MotdRepository : IMotdRepository
    {
        private DatabaseContext context;

        public MotdRepository(DatabaseContext context)
        {
            this.context = context;
        }

        public async Task<DbMotd> GetLastMessageOfTheDay()
        {
            await Task.Yield();

            return await context.Motd
                .OrderByDescending(m => m.Id)
                .FirstOrDefaultAsync();
        }

        public void AddMessageOfTheDay(DbMotd motd)
        {
            context.Motd.Add(motd);
        }
    }
}