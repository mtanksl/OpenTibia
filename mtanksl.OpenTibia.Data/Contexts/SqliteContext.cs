using Microsoft.EntityFrameworkCore;
using OpenTibia.Data.Models;

namespace OpenTibia.Data.Contexts
{
    public class SqliteContext : DbContext
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbPlayerItem>()
                .HasKey(m => new { m.PlayerId, m.SequenceId } );

            modelBuilder.Entity<DbPlayerDepotItem>()
                .HasKey(m => new { m.PlayerId, m.SequenceId } );

            modelBuilder.Entity<DbPlayerVip>()
                .HasKey(m => new { m.PlayerId, m.SequenceId } );

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

        public DbSet<DbPlayer> Players { get; set; }

        public DbSet<DbPlayerItem> PlayerItems { get; set; }

        public DbSet<DbPlayerDepotItem> PlayerDepotItems { get; set; }

        public DbSet<DbPlayerVip> PlayerVips { get; set; }

        public DbSet<DbWorld> Worlds { get; set; }
    }
}