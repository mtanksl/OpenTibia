using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OpenTibia.Data.PostgreSql.Contexts
{
    public class PostgreSqlContextFactory : IDesignTimeDbContextFactory<PostgreSqlContext>
    {
        public PostgreSqlContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PostgreSqlContext>();

            return new PostgreSqlContext(args[0], optionsBuilder.Options);
        }
    }
}