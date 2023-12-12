using Microsoft.EntityFrameworkCore;
using OpenTibia.Data.Contexts;

namespace OpenTibia.Data.MsSql.Contexts
{
    public class MsSqlContext : DatabaseContext
    {
        private string connectionString;

        public MsSqlContext(string connectionString) : base()
        {
            this.connectionString = connectionString;
        }

        public MsSqlContext(string connectionString, DbContextOptions options) : base(options)
        {
            this.connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);

            base.OnConfiguring(optionsBuilder);
        }
    }
}