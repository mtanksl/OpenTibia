using Microsoft.EntityFrameworkCore;
using OpenTibia.Data.Models;

namespace OpenTibia.Data.Contexts
{
    public abstract class DatabaseContext : DbContext
    {
        public DatabaseContext() : base()
        {

        }

        public DatabaseContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbPlayerDepotItem>()
                .HasKey(m => new { m.PlayerId, m.SequenceId } );

            modelBuilder.Entity<DbPlayerItem>()
                .HasKey(m => new { m.PlayerId, m.SequenceId } );

            modelBuilder.Entity<DbPlayerStorage>()
                .HasKey(m => new { m.PlayerId, m.Key } );

            modelBuilder.Entity<DbPlayerVip>()
                .HasKey(m => new { m.PlayerId, m.VipId } );

            modelBuilder.Entity<DbPlayerVip>()
                .HasOne(v => v.Player)
                .WithMany(p => p.PlayerVips)
                .HasForeignKey(v => v.PlayerId);

            modelBuilder.Entity<DbPlayerVip>()
                .HasOne(v => v.Vip)
                .WithMany()
                .HasForeignKey(v => v.VipId);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<DbAccount> Accounts { get; set; }

        public DbSet<DbBan> Bans { get; set; }

        public DbSet<DbBugReport> BugReports { get; set; }

        public DbSet<DbMotd> Motd { get; set; }

        public DbSet<DbPlayer> Players { get; set; }

        public DbSet<DbPlayerDepotItem> PlayerDepotItems { get; set; }

        public DbSet<DbPlayerItem> PlayerItems { get; set; }

        public DbSet<DbPlayerStorage> PlayerStorages { get; set; }

        public DbSet<DbPlayerVip> PlayerVips { get; set; }

        public DbSet<DbRuleViolationReport> RuleViolationReports { get; set; }

        public DbSet<DbWorld> Worlds { get; set; }
    }
}