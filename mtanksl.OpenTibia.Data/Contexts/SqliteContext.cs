using OpenTibia.Data.Migrations;
using OpenTibia.Data.Models;
using System.Data.Entity;

namespace OpenTibia.Data.Contexts
{
    public class SqliteContext : DbContext
    {
        public SqliteContext() : base("SqliteContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<SqliteContext, SqliteContextMigrationConfiguration>(true) );
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlayerItem>()
                .HasKey(m => new { m.PlayerId, m.SequenceId } );

            modelBuilder.Entity<PlayerDepotItem>()
                .HasKey(m => new { m.PlayerId, m.SequenceId } );

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<PlayerItem> PlayerItems { get; set; }

        public DbSet<PlayerDepotItem> PlayerDepotItems { get; set; }

        public DbSet<World> Worlds { get; set; }
    }
}