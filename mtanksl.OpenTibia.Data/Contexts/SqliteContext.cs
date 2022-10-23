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

            modelBuilder.Entity<PlayerVip>()
                .HasKey(m => new { m.PlayerId, m.SequenceId } );

            modelBuilder.Entity<PlayerVip>()
                .HasRequired(v => v.Player)
                .WithMany(p => p.PlayerVips)
                .HasForeignKey(v => v.PlayerId);

            modelBuilder.Entity<PlayerVip>()
                .HasRequired(v => v.Vip)
                .WithMany()
                .HasForeignKey(v => v.VipId);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<PlayerItem> PlayerItems { get; set; }

        public DbSet<PlayerDepotItem> PlayerDepotItems { get; set; }

        public DbSet<PlayerVip> PlayerVips { get; set; }

        public DbSet<World> Worlds { get; set; }
    }
}