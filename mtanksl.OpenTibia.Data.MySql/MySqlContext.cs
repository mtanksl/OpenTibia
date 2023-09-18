using Microsoft.EntityFrameworkCore;
using OpenTibia.Data.Contexts;

namespace mtanksl.OpenTibia.Data.MySql
{
    public class MySqlContext : DatabaseContext
    {
        private string connectionString;

        public MySqlContext(string connectionString) : base()
        {
            this.connectionString = connectionString;
        }

        public MySqlContext(string connectionString, DbContextOptions options) : base(options)
        {
            this.connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString) );

            base.OnConfiguring(optionsBuilder);
        }
    }
}