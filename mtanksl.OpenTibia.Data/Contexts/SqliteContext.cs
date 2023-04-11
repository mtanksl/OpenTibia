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
            modelBuilder.Entity<PlayerItem>()
                .HasKey(m => new { m.PlayerId, m.SequenceId } );

            modelBuilder.Entity<PlayerDepotItem>()
                .HasKey(m => new { m.PlayerId, m.SequenceId } );

            modelBuilder.Entity<PlayerVip>()
                .HasKey(m => new { m.PlayerId, m.SequenceId } );

            modelBuilder.Entity<PlayerVip>()
                .HasOne(v => v.Player)
                .WithMany(p => p.PlayerVips)
                .HasForeignKey(v => v.PlayerId);

            modelBuilder.Entity<PlayerVip>()
                .HasOne(v => v.Vip)
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