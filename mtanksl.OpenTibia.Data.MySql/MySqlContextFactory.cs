using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OpenTibia.Data.MySql.Contexts
{
    public class MySqlContextFactory : IDesignTimeDbContextFactory<MySqlContext>
    {
        public MySqlContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MySqlContext>();

            return new MySqlContext(args[0], optionsBuilder.Options);
        }
    }
}