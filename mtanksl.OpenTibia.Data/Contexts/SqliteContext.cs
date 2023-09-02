using Microsoft.EntityFrameworkCore;

namespace OpenTibia.Data.Contexts
{
    public class SqliteContext : DatabaseContext
    {
        private string dataSource;

        public SqliteContext(string dataSource) : base()
        {
            this.dataSource = dataSource;
        }

        public SqliteContext(string dataSource, DbContextOptions<SqliteContext> options) : base(options)
        {
            this.dataSource = dataSource;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=" + dataSource);

            base.OnConfiguring(optionsBuilder);
        }
    }
}