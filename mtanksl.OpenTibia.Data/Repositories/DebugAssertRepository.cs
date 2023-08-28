using OpenTibia.Data.Contexts;
using OpenTibia.Data.Models;

namespace OpenTibia.Data.Repositories
{
    public class DebugAssertRepository
    {
        private DatabaseContext context;

        public DebugAssertRepository(DatabaseContext context)
        {
            this.context = context;
        }

        public void AddDebugAssert(DbDebugAssert debugAssert)
        {
            context.DebugAsserts.Add(debugAssert);
        }
    }
}