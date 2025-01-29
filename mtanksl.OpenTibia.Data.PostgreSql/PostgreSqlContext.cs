using Microsoft.EntityFrameworkCore;
using OpenTibia.Data.Contexts;

namespace OpenTibia.Data.PostgreSql.Contexts
{
    public class PostgreSqlContext : DatabaseContext
    {
        private string connectionString;

        public PostgreSqlContext(string connectionString) : base()
        {
            this.connectionString = connectionString;
        }

        public PostgreSqlContext(string connectionString, DbContextOptions options) : base(options)
        {
            this.connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(connectionString);

            base.OnConfiguring(optionsBuilder);
        }
    }
}