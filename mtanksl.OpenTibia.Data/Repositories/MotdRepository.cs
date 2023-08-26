using OpenTibia.Data.Contexts;
using OpenTibia.Data.Models;
using System.Linq;

namespace OpenTibia.Data.Repositories
{
    public class MotdRepository
    {
        private DatabaseContext context;

        public MotdRepository(DatabaseContext context)
        {
            this.context = context;
        }

        public DbMotd GetMotd()
        {
            return context.Motd
                .OrderByDescending(m => m.Id)
                .FirstOrDefault();
        }
    }
}