using Microsoft.EntityFrameworkCore;

namespace OpenTibia.Data.Contexts
{
    public class SqliteContext : DatabaseContext
    {
        public SqliteContext() : base()
        {

        }

        public SqliteContext(DbContextOptions<SqliteContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=.\\data\\database.db");

            base.OnConfiguring(optionsBuilder);
        }
    }
}