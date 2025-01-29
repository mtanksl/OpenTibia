using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OpenTibia.Data.MsSql.Contexts
{
    public class MsSqlContextFactory : IDesignTimeDbContextFactory<MsSqlContext>
    {
        public MsSqlContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MsSqlContext>();

            return new MsSqlContext(args[0], optionsBuilder.Options);
        }
    }
}