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
                .HasForeignKey(v => v.AttackerId)
                .OnDelete(DeleteBehavior.NoAction);

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
                .HasForeignKey(v => v.TargetId)
                .OnDelete(DeleteBehavior.NoAction);

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
                .HasForeignKey(v => v.VipId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<DbServerStorage>()
                .HasKey(m => m.Key);

            if (EF.IsDesignTime)
            {
                modelBuilder.Entity<DbAccount>().HasData(

                    new DbAccount() { Id = 1, Name = "1", Password = "1", PremiumUntil = null } );

                modelBuilder.Entity<DbMotd>().HasData(

                    new DbMotd() { Id = 1, Message = "MTOTS - An open Tibia server developed by mtanksl" } );

                modelBuilder.Entity<DbPlayerItem>().HasData(

                    new DbPlayerItem() { PlayerId = 1, SequenceId = 101, ParentId = 3, OpenTibiaId = 1987, Count = 1 },
                    new DbPlayerItem() { PlayerId = 1, SequenceId = 102, ParentId = 101, OpenTibiaId = 2120, Count = 1 },
                    new DbPlayerItem() { PlayerId = 1, SequenceId = 103, ParentId = 101, OpenTibiaId = 2554, Count = 1 },
                    new DbPlayerItem() { PlayerId = 2, SequenceId = 101, ParentId = 3, OpenTibiaId = 1987, Count = 1 },
                    new DbPlayerItem() { PlayerId = 2, SequenceId = 102, ParentId = 101, OpenTibiaId = 2120, Count = 1 },
                    new DbPlayerItem() { PlayerId = 2, SequenceId = 103, ParentId = 101, OpenTibiaId = 2554, Count = 1 },
                    new DbPlayerItem() { PlayerId = 3, SequenceId = 101, ParentId = 3, OpenTibiaId = 1987, Count = 1 },
                    new DbPlayerItem() { PlayerId = 3, SequenceId = 102, ParentId = 101, OpenTibiaId = 2120, Count = 1 },
                    new DbPlayerItem() { PlayerId = 3, SequenceId = 103, ParentId = 101, OpenTibiaId = 2554, Count = 1 },
                    new DbPlayerItem() { PlayerId = 4, SequenceId = 101, ParentId = 3, OpenTibiaId = 1987, Count = 1 },
                    new DbPlayerItem() { PlayerId = 4, SequenceId = 102, ParentId = 101, OpenTibiaId = 2120, Count = 1 },
                    new DbPlayerItem() { PlayerId = 4, SequenceId = 103, ParentId = 101, OpenTibiaId = 2554, Count = 1 },
                    new DbPlayerItem() { PlayerId = 5, SequenceId = 101, ParentId = 3, OpenTibiaId = 1987, Count = 1 },
                    new DbPlayerItem() { PlayerId = 5, SequenceId = 102, ParentId = 101, OpenTibiaId = 2120, Count = 1 },
                    new DbPlayerItem() { PlayerId = 5, SequenceId = 103, ParentId = 101, OpenTibiaId = 2554, Count = 1 } );

                modelBuilder.Entity<DbPlayerOutfit>().HasData(

                    new DbPlayerOutfit() { PlayerId = 1, OutfitId = 128, OutfitAddon = 0 },
                    new DbPlayerOutfit() { PlayerId = 1, OutfitId = 129, OutfitAddon = 0 },
                    new DbPlayerOutfit() { PlayerId = 1, OutfitId = 130, OutfitAddon = 0 },
                    new DbPlayerOutfit() { PlayerId = 1, OutfitId = 131, OutfitAddon = 0 },
                    new DbPlayerOutfit() { PlayerId = 2, OutfitId = 128, OutfitAddon = 0 },
                    new DbPlayerOutfit() { PlayerId = 2, OutfitId = 129, OutfitAddon = 0 },
                    new DbPlayerOutfit() { PlayerId = 2, OutfitId = 130, OutfitAddon = 0 },
                    new DbPlayerOutfit() { PlayerId = 2, OutfitId = 131, OutfitAddon = 0 },
                    new DbPlayerOutfit() { PlayerId = 3, OutfitId = 128, OutfitAddon = 0 },
                    new DbPlayerOutfit() { PlayerId = 3, OutfitId = 129, OutfitAddon = 0 },
                    new DbPlayerOutfit() { PlayerId = 3, OutfitId = 130, OutfitAddon = 0 },
                    new DbPlayerOutfit() { PlayerId = 3, OutfitId = 131, OutfitAddon = 0 },
                    new DbPlayerOutfit() { PlayerId = 4, OutfitId = 128, OutfitAddon = 0 },
                    new DbPlayerOutfit() { PlayerId = 4, OutfitId = 129, OutfitAddon = 0 },
                    new DbPlayerOutfit() { PlayerId = 4, OutfitId = 130, OutfitAddon = 0 },
                    new DbPlayerOutfit() { PlayerId = 4, OutfitId = 131, OutfitAddon = 0 },
                    new DbPlayerOutfit() { PlayerId = 5, OutfitId = 128, OutfitAddon = 0 },
                    new DbPlayerOutfit() { PlayerId = 5, OutfitId = 129, OutfitAddon = 0 },
                    new DbPlayerOutfit() { PlayerId = 5, OutfitId = 130, OutfitAddon = 0 },
                    new DbPlayerOutfit() { PlayerId = 5, OutfitId = 131, OutfitAddon = 0 } );
                               
                modelBuilder.Entity<DbPlayer>().HasData(

                    new DbPlayer() { Id = 1, AccountId = 1, WorldId = 1, Name = "Gamemaster", Health = 645, MaxHealth = 645, Direction = 2, BaseOutfitId = 266, BaseSpeed = 2218, SkillMagicLevel = 0, SkillFist = 10, SkillClub = 10, SkillSword = 10, SkillAxe = 10, SkillDistance = 10, SkillShield = 10, SkillFish = 10, Experience = 15694800, Level = 100, Mana = 550, MaxMana = 550, Soul = 100, Capacity = 139000, MaxCapacity = 139000, Stamina = 2520, Rank = 2, SpawnX = 921, SpawnY = 771, SpawnZ = 6, TownX = 921, TownY = 771, TownZ = 6 },
                    new DbPlayer() { Id = 2, AccountId = 1, WorldId = 1, Name = "Knight", Health = 1565, MaxHealth = 1565, Direction = 2, BaseOutfitId = 131, BaseSpeed = 418, SkillMagicLevel = 4, SkillFist = 10, SkillClub = 10, SkillSword = 90, SkillAxe = 10, SkillDistance = 10, SkillShield = 80, SkillFish = 10, Experience = 15694800, Level = 100, Mana = 550, MaxMana = 550, Soul = 100, Capacity = 139000, MaxCapacity = 139000, Stamina = 2520, Vocation = 1, SpawnX = 921, SpawnY = 771, SpawnZ = 6, TownX = 921, TownY = 771, TownZ = 6 },
                    new DbPlayer() { Id = 3, AccountId = 1, WorldId = 1, Name = "Paladin", Health = 1105, MaxHealth = 1105, Direction = 2, BaseOutfitId = 129, BaseSpeed = 418, SkillMagicLevel = 15, SkillFist = 10, SkillClub = 10, SkillSword = 10, SkillAxe = 10, SkillDistance = 70, SkillShield = 40, SkillFish = 10, Experience = 15694800, Level = 100, Mana = 1470, MaxMana = 1470, Soul = 100, Capacity = 139000, MaxCapacity = 139000, Stamina = 2520, Vocation = 2, SpawnX = 921, SpawnY = 771, SpawnZ = 6, TownX = 921, TownY = 771, TownZ = 6 },
                    new DbPlayer() { Id = 4, AccountId = 1, WorldId = 1, Name = "Sorcerer", Health = 645, MaxHealth = 645, Direction = 2, BaseOutfitId = 130, BaseSpeed = 418,SkillMagicLevel = 60, SkillFist = 10, SkillClub = 10, SkillSword = 10, SkillAxe = 10, SkillDistance = 10, SkillShield = 10, SkillFish = 10, Experience = 15694800, Level = 100, Mana = 2850, MaxMana = 2850, Soul = 100, Capacity = 139000, MaxCapacity = 139000, Stamina = 2520, Vocation = 4, SpawnX = 921, SpawnY = 771, SpawnZ = 6, TownX = 921, TownY = 771, TownZ = 6 },
                    new DbPlayer() { Id = 5, AccountId = 1, WorldId = 1, Name = "Druid", Health = 645, MaxHealth = 645, Direction = 2, BaseOutfitId = 130, BaseSpeed = 418, SkillMagicLevel = 60, SkillFist = 10, SkillClub = 10, SkillSword = 10, SkillAxe = 10, SkillDistance = 10, SkillShield = 10, SkillFish = 10, Experience = 15694800, Level = 100, Mana = 2850, MaxMana = 2850, Soul = 100, Capacity = 139000, MaxCapacity = 139000, Stamina = 2520, Vocation = 3, SpawnX = 921, SpawnY = 771, SpawnZ = 6, TownX = 921, TownY = 771, TownZ = 6 } );

                modelBuilder.Entity<DbServerStorage>().HasData(

                   new DbServerStorage() { Key = "PlayersPeek", Value = "0" } );

                modelBuilder.Entity<DbWorld>().HasData(

                    new DbWorld() { Id = 1, Name = "Cormaya", Ip = "127.0.0.1", Port = 7172 } );
            }

            base.OnModelCreating(modelBuilder);
        }

        public void Seed()
        {
            Accounts.Add(

                new DbAccount() { Id = 1, Name = "1", Password = "1", PremiumUntil = null } );

            Motd.Add(

                new DbMotd() { Id = 1, Message = "MTOTS - An open Tibia server developed by mtanksl" } );

            PlayerItems.AddRange(

                new DbPlayerItem() { PlayerId = 1, SequenceId = 101, ParentId = 3, OpenTibiaId = 1987, Count = 1 },
                new DbPlayerItem() { PlayerId = 1, SequenceId = 102, ParentId = 101, OpenTibiaId = 2120, Count = 1 },
                new DbPlayerItem() { PlayerId = 1, SequenceId = 103, ParentId = 101, OpenTibiaId = 2554, Count = 1 },
                new DbPlayerItem() { PlayerId = 2, SequenceId = 101, ParentId = 3, OpenTibiaId = 1987, Count = 1 },
                new DbPlayerItem() { PlayerId = 2, SequenceId = 102, ParentId = 101, OpenTibiaId = 2120, Count = 1 },
                new DbPlayerItem() { PlayerId = 2, SequenceId = 103, ParentId = 101, OpenTibiaId = 2554, Count = 1 },
                new DbPlayerItem() { PlayerId = 3, SequenceId = 101, ParentId = 3, OpenTibiaId = 1987, Count = 1 },
                new DbPlayerItem() { PlayerId = 3, SequenceId = 102, ParentId = 101, OpenTibiaId = 2120, Count = 1 },
                new DbPlayerItem() { PlayerId = 3, SequenceId = 103, ParentId = 101, OpenTibiaId = 2554, Count = 1 },
                new DbPlayerItem() { PlayerId = 4, SequenceId = 101, ParentId = 3, OpenTibiaId = 1987, Count = 1 },
                new DbPlayerItem() { PlayerId = 4, SequenceId = 102, ParentId = 101, OpenTibiaId = 2120, Count = 1 },
                new DbPlayerItem() { PlayerId = 4, SequenceId = 103, ParentId = 101, OpenTibiaId = 2554, Count = 1 },
                new DbPlayerItem() { PlayerId = 5, SequenceId = 101, ParentId = 3, OpenTibiaId = 1987, Count = 1 },
                new DbPlayerItem() { PlayerId = 5, SequenceId = 102, ParentId = 101, OpenTibiaId = 2120, Count = 1 },
                new DbPlayerItem() { PlayerId = 5, SequenceId = 103, ParentId = 101, OpenTibiaId = 2554, Count = 1 } );

            PlayerOutfits.AddRange(

                new DbPlayerOutfit() { PlayerId = 1, OutfitId = 128, OutfitAddon = 0 },
                new DbPlayerOutfit() { PlayerId = 1, OutfitId = 129, OutfitAddon = 0 },
                new DbPlayerOutfit() { PlayerId = 1, OutfitId = 130, OutfitAddon = 0 },
                new DbPlayerOutfit() { PlayerId = 1, OutfitId = 131, OutfitAddon = 0 },
                new DbPlayerOutfit() { PlayerId = 2, OutfitId = 128, OutfitAddon = 0 },
                new DbPlayerOutfit() { PlayerId = 2, OutfitId = 129, OutfitAddon = 0 },
                new DbPlayerOutfit() { PlayerId = 2, OutfitId = 130, OutfitAddon = 0 },
                new DbPlayerOutfit() { PlayerId = 2, OutfitId = 131, OutfitAddon = 0 },
                new DbPlayerOutfit() { PlayerId = 3, OutfitId = 128, OutfitAddon = 0 },
                new DbPlayerOutfit() { PlayerId = 3, OutfitId = 129, OutfitAddon = 0 },
                new DbPlayerOutfit() { PlayerId = 3, OutfitId = 130, OutfitAddon = 0 },
                new DbPlayerOutfit() { PlayerId = 3, OutfitId = 131, OutfitAddon = 0 },
                new DbPlayerOutfit() { PlayerId = 4, OutfitId = 128, OutfitAddon = 0 },
                new DbPlayerOutfit() { PlayerId = 4, OutfitId = 129, OutfitAddon = 0 },
                new DbPlayerOutfit() { PlayerId = 4, OutfitId = 130, OutfitAddon = 0 },
                new DbPlayerOutfit() { PlayerId = 4, OutfitId = 131, OutfitAddon = 0 },
                new DbPlayerOutfit() { PlayerId = 5, OutfitId = 128, OutfitAddon = 0 },
                new DbPlayerOutfit() { PlayerId = 5, OutfitId = 129, OutfitAddon = 0 },
                new DbPlayerOutfit() { PlayerId = 5, OutfitId = 130, OutfitAddon = 0 },
                new DbPlayerOutfit() { PlayerId = 5, OutfitId = 131, OutfitAddon = 0 } );
                               
            Players.AddRange(

                new DbPlayer() { Id = 1, AccountId = 1, WorldId = 1, Name = "Gamemaster", Health = 645, MaxHealth = 645, Direction = 2, BaseOutfitId = 266, BaseSpeed = 2218, SkillMagicLevel = 0, SkillFist = 10, SkillClub = 10, SkillSword = 10, SkillAxe = 10, SkillDistance = 10, SkillShield = 10, SkillFish = 10, Experience = 15694800, Level = 100, Mana = 550, MaxMana = 550, Soul = 100, Capacity = 139000, MaxCapacity = 139000, Stamina = 2520, Rank = 2, SpawnX = 921, SpawnY = 771, SpawnZ = 6, TownX = 921, TownY = 771, TownZ = 6 },
                new DbPlayer() { Id = 2, AccountId = 1, WorldId = 1, Name = "Knight", Health = 1565, MaxHealth = 1565, Direction = 2, BaseOutfitId = 131, BaseSpeed = 418, SkillMagicLevel = 4, SkillFist = 10, SkillClub = 10, SkillSword = 90, SkillAxe = 10, SkillDistance = 10, SkillShield = 80, SkillFish = 10, Experience = 15694800, Level = 100, Mana = 550, MaxMana = 550, Soul = 100, Capacity = 139000, MaxCapacity = 139000, Stamina = 2520, Vocation = 1, SpawnX = 921, SpawnY = 771, SpawnZ = 6, TownX = 921, TownY = 771, TownZ = 6 },
                new DbPlayer() { Id = 3, AccountId = 1, WorldId = 1, Name = "Paladin", Health = 1105, MaxHealth = 1105, Direction = 2, BaseOutfitId = 129, BaseSpeed = 418, SkillMagicLevel = 15, SkillFist = 10, SkillClub = 10, SkillSword = 10, SkillAxe = 10, SkillDistance = 70, SkillShield = 40, SkillFish = 10, Experience = 15694800, Level = 100, Mana = 1470, MaxMana = 1470, Soul = 100, Capacity = 139000, MaxCapacity = 139000, Stamina = 2520, Vocation = 2, SpawnX = 921, SpawnY = 771, SpawnZ = 6, TownX = 921, TownY = 771, TownZ = 6 },
                new DbPlayer() { Id = 4, AccountId = 1, WorldId = 1, Name = "Sorcerer", Health = 645, MaxHealth = 645, Direction = 2, BaseOutfitId = 130, BaseSpeed = 418, SkillMagicLevel = 60, SkillFist = 10, SkillClub = 10, SkillSword = 10, SkillAxe = 10, SkillDistance = 10, SkillShield = 10, SkillFish = 10, Experience = 15694800, Level = 100, Mana = 2850, MaxMana = 2850, Soul = 100, Capacity = 139000, MaxCapacity = 139000, Stamina = 2520, Vocation = 4, SpawnX = 921, SpawnY = 771, SpawnZ = 6, TownX = 921, TownY = 771, TownZ = 6 },
                new DbPlayer() { Id = 5, AccountId = 1, WorldId = 1, Name = "Druid", Health = 645, MaxHealth = 645, Direction = 2, BaseOutfitId = 130, BaseSpeed = 418, SkillMagicLevel = 60, SkillFist = 10, SkillClub = 10, SkillSword = 10, SkillAxe = 10, SkillDistance = 10, SkillShield = 10, SkillFish = 10, Experience = 15694800, Level = 100, Mana = 2850, MaxMana = 2850, Soul = 100, Capacity = 139000, MaxCapacity = 139000, Stamina = 2520, Vocation = 3, SpawnX = 921, SpawnY = 771, SpawnZ = 6, TownX = 921, TownY = 771, TownZ = 6 } );

            ServerStorages.Add(

                new DbServerStorage() { Key = "PlayersPeek", Value = "0" } );

            Worlds.Add(

                new DbWorld() { Id = 1, Name = "Cormaya", Ip = "127.0.0.1", Port = 7172 } );
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