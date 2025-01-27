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
            modelBuilder.Entity<DbHouseAccessList>()
                .HasKey(m => new { m.HouseId, m.ListId } );

            modelBuilder.Entity<DbHouseItem>()
                .HasKey(m => new { m.HouseId, m.SequenceId } );

            modelBuilder.Entity<DbPlayerAchievement>()
                .HasKey(m => new { m.PlayerId, m.Name } );

            modelBuilder.Entity<DbPlayerBless>()
                .HasKey(m => new { m.PlayerId, m.Name } );

            modelBuilder.Entity<DbPlayerDeath>()
                .HasOne(v => v.Player)
                .WithMany(p => p.PlayerDeaths)
                .HasForeignKey(v => v.PlayerId);

            modelBuilder.Entity<DbPlayerDeath>()
                .HasOne(v => v.Attacker)
                .WithMany()
                .HasForeignKey(v => v.AttackerId);

            modelBuilder.Entity<DbPlayerDepotItem>()
                .HasKey(m => new { m.PlayerId, m.SequenceId } );

            modelBuilder.Entity<DbPlayerItem>()
                .HasKey(m => new { m.PlayerId, m.SequenceId } );

            modelBuilder.Entity<DbPlayerKill>()
                .HasOne(v => v.Player)
                .WithMany(p => p.PlayerKills)
                .HasForeignKey(v => v.PlayerId);

            modelBuilder.Entity<DbPlayerKill>()
                .HasOne(v => v.Target)
                .WithMany()
                .HasForeignKey(v => v.TargetId);

            modelBuilder.Entity<DbPlayerOutfit>()
                .HasKey(m => new { m.PlayerId, m.OutfitId } );

            modelBuilder.Entity<DbPlayerSpell>()
                .HasKey(m => new { m.PlayerId, m.Name } );

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

            modelBuilder.Entity<DbServerStorage>()
                .HasKey(m => m.Key);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<DbAccount> Accounts { get; set; }

        public DbSet<DbBan> Bans { get; set; }

        public DbSet<DbBugReport> BugReports { get; set; }

        public DbSet<DbDebugAssert> DebugAsserts { get; set; }

        public DbSet<DbHouse> Houses { get; set; }

        public DbSet<DbHouseAccessList> HouseAccessLists { get; set; }

        public DbSet<DbHouseItem> HouseItems { get; set; }

        public DbSet<DbMotd> Motd { get; set; }

        public DbSet<DbPlayer> Players { get; set; }

        public DbSet<DbPlayerAchievement> PlayerAchievements { get; set; }

        public DbSet<DbPlayerBless> PlayerBlesses { get; set; }

        public DbSet<DbPlayerDeath> PlayerDeaths { get; set; }

        public DbSet<DbPlayerDepotItem> PlayerDepotItems { get; set; }

        public DbSet<DbPlayerItem> PlayerItems { get; set; }

        public DbSet<DbPlayerKill> PlayerKills { get; set; }

        public DbSet<DbPlayerOutfit> PlayerOutfits { get; set; }

        public DbSet<DbPlayerSpell> PlayerSpells { get; set; }

        public DbSet<DbPlayerStorage> PlayerStorages { get; set; }

        public DbSet<DbPlayerVip> PlayerVips { get; set; }
                
        public DbSet<DbRuleViolationReport> RuleViolationReports { get; set; }

        public DbSet<DbServerStorage> ServerStorages { get; set; }

        public DbSet<DbWorld> Worlds { get; set; }
    }
}