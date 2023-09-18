using Microsoft.EntityFrameworkCore;
using OpenTibia.Data.Contexts;

namespace OpenTibia.Data.Sqlite.Contexts
{
    public class SqliteContext : DatabaseContext
    {
        private string connectionString;

        public SqliteContext(string connectionString) : base()
        {
            this.connectionString = connectionString;
        }

        public SqliteContext(string connectionString, DbContextOptions options) : base(options)
        {
            this.connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(connectionString);

            base.OnConfiguring(optionsBuilder);
        }
    }
}