namespace OpenTibia.Data.Migrations
{
    using OpenTibia.Common.Structures;
    using OpenTibia.Data.Contexts;
    using OpenTibia.Data.Models;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Data.SQLite.EF6.Migrations;
    using System.Linq;

    internal sealed class SqliteContextMigrationConfiguration : DbMigrationsConfiguration<SqliteContext>
    {
        public SqliteContextMigrationConfiguration()
        {
            AutomaticMigrationsEnabled = true;

            AutomaticMigrationDataLossAllowed = true;

            SetSqlGenerator("System.Data.SQLite", new SQLiteMigrationSqlGenerator() );
        }

        protected override void Seed(SqliteContext context)
        {
            var world = context.Worlds.Where(w => w.Id == 1).FirstOrDefault();

            if (world == null)
            {
                world = new World() 
                { 
                    Id = 1,
                    Name = "World", 
                    Ip = "127.0.0.1",
                    Port = 7172 
                };

                context.Worlds.AddOrUpdate(world);
            }

            var account = context.Accounts.Where(w => w.Id == 1).FirstOrDefault();

            if (account == null)
            {
                account = new Account()
                {
                    Id = 1, 
                    Name = "1",
                    Password = "1", 
                    PremiumDays = 0 
                };

                context.Accounts.AddOrUpdate(account);

                context.Players.AddOrUpdate(new Player() 
                {
                    Id = 1, 
                    AccountId = account.Id, 
                    WorldId = world.Id, 
                    Name = "Gamemaster",
                    Health = 645,
                    MaxHealth = 645,
                    Direction = 2,
                    OutfitId = 266,
                    OutfitHead = 0,
                    OutfitBody = 0,
                    OutfitLegs = 0,
                    OutfitFeet = 0,
                    OutfitAddon = 0,
                    BaseSpeed = 2218,
                    Speed = 2218,
                    SkillMagicLevel = 0,
                    SkillMagicLevelPercent = 0,
                    SkillFist = 0,
                    SkillFistPercent = 0,
                    SkillClub = 0,
                    SkillClubPercent = 0,
                    SkillSword = 0,
                    SkillSwordPercent = 0,
                    SkillAxe = 0,
                    SkillAxePercent = 0,
                    SkillDistance = 0,
                    SkillDistancePercent = 0,
                    SkillShield = 0,
                    SkillShieldPercent = 0,
                    SkillFish = 0,
                    SkillFishPercent = 0,
                    Experience = 15694800,
                    Level = 100,
                    LevelPercent = 0,
                    Mana = 550,
                    MaxMana = 550,
                    Soul = 100,
                    Capacity = 1390 * 100,
                    Stamina = 42 * 60,
                    Gender = (int)Gender.Male,
                    Vocation = (int)Vocation.None,
                    CoordinateX = 930, 
                    CoordinateY = 779, 
                    CoordinateZ = 7,
                    PlayerItems = new List<PlayerItem>()
                    {
                        new PlayerItem() { SequenceId = 101, ParentId = 3, OpenTibiaId = 1987, Count = 1 },
                        new PlayerItem() { SequenceId = 102, ParentId = 101, OpenTibiaId = 2120, Count = 1 },
                        new PlayerItem() { SequenceId = 103, ParentId = 101, OpenTibiaId = 2554, Count = 1 }
                    }
                } );

                context.Players.AddOrUpdate(new Player() 
                {
                    Id = 2, 
                    AccountId = account.Id, 
                    WorldId = world.Id, 
                    Name = "Knight",
                    Health = 1565,
                    MaxHealth = 1565,
                    Direction = 2,
                    OutfitId = 131,
                    OutfitHead = 0,
                    OutfitBody = 0,
                    OutfitLegs = 0,
                    OutfitFeet = 0,
                    OutfitAddon = 0,
                    BaseSpeed = 418,
                    Speed = 418,
                    SkillMagicLevel = 4,
                    SkillMagicLevelPercent = 0,
                    SkillFist = 0,
                    SkillFistPercent = 0,
                    SkillClub = 0,
                    SkillClubPercent = 0,
                    SkillSword = 90,
                    SkillSwordPercent = 0,
                    SkillAxe = 0,
                    SkillAxePercent = 0,
                    SkillDistance = 0,
                    SkillDistancePercent = 0,
                    SkillShield = 80,
                    SkillShieldPercent = 0,
                    SkillFish = 0,
                    SkillFishPercent = 0,
                    Experience = 15694800,
                    Level = 100,
                    LevelPercent = 0,
                    Mana = 550,
                    MaxMana = 550,
                    Soul = 100,
                    Capacity = 2770 * 100,
                    Stamina = 42 * 60,
                    Gender = (int)Gender.Male,
                    Vocation = (int)Vocation.Knight,
                    CoordinateX = 930, 
                    CoordinateY = 779, 
                    CoordinateZ = 7,
                    PlayerItems = new List<PlayerItem>()
                    {
                        new PlayerItem() { SequenceId = 101, ParentId = 3, OpenTibiaId = 1987, Count = 1 },
                        new PlayerItem() { SequenceId = 102, ParentId = 101, OpenTibiaId = 2120, Count = 1 },
                        new PlayerItem() { SequenceId = 103, ParentId = 101, OpenTibiaId = 2554, Count = 1 }
                    }
                } );

                context.Players.AddOrUpdate(new Player() 
                {
                    Id = 3, 
                    AccountId = account.Id, 
                    WorldId = world.Id, 
                    Name = "Paladin",
                    Health = 1105,
                    MaxHealth = 1105,
                    Direction = 2,
                    OutfitId = 129,
                    OutfitHead = 0,
                    OutfitBody = 0,
                    OutfitLegs = 0,
                    OutfitFeet = 0,
                    OutfitAddon = 0,
                    BaseSpeed = 418,
                    Speed = 418,
                    SkillMagicLevel = 20,
                    SkillMagicLevelPercent = 0,
                    SkillFist = 0,
                    SkillFistPercent = 0,
                    SkillClub = 0,
                    SkillClubPercent = 0,
                    SkillSword = 0,
                    SkillSwordPercent = 0,
                    SkillAxe = 0,
                    SkillAxePercent = 0,
                    SkillDistance = 70,
                    SkillDistancePercent = 0,
                    SkillShield = 40,
                    SkillShieldPercent = 0,
                    SkillFish = 0,
                    SkillFishPercent = 0,
                    Experience = 15694800,
                    Level = 100,
                    LevelPercent = 0,
                    Mana = 1470,
                    MaxMana = 1470,
                    Soul = 100,
                    Capacity = 2310 * 100,
                    Stamina = 42 * 60,
                    Gender = (int)Gender.Male,
                    Vocation = (int)Vocation.Paladin,
                    CoordinateX = 930, 
                    CoordinateY = 779, 
                    CoordinateZ = 7,
                    PlayerItems = new List<PlayerItem>()
                    {
                        new PlayerItem() { SequenceId = 101, ParentId = 3, OpenTibiaId = 1987, Count = 1 },
                        new PlayerItem() { SequenceId = 102, ParentId = 101, OpenTibiaId = 2120, Count = 1 },
                        new PlayerItem() { SequenceId = 103, ParentId = 101, OpenTibiaId = 2554, Count = 1 }
                    }
                } );

                context.Players.AddOrUpdate(new Player() 
                {
                    Id = 4, 
                    AccountId = account.Id, 
                    WorldId = world.Id, 
                    Name = "Sorcerer",
                    Health = 645,
                    MaxHealth = 645,
                    Direction = 2,
                    OutfitId = 130,
                    OutfitHead = 0,
                    OutfitBody = 0,
                    OutfitLegs = 0,
                    OutfitFeet = 0,
                    OutfitAddon = 0,
                    BaseSpeed = 418,
                    Speed = 418,
                    SkillMagicLevel = 70,
                    SkillMagicLevelPercent = 0,
                    SkillFist = 0,
                    SkillFistPercent = 0,
                    SkillClub = 0,
                    SkillClubPercent = 0,
                    SkillSword = 0,
                    SkillSwordPercent = 0,
                    SkillAxe = 0,
                    SkillAxePercent = 0,
                    SkillDistance = 0,
                    SkillDistancePercent = 0,
                    SkillShield = 0,
                    SkillShieldPercent = 0,
                    SkillFish = 0,
                    SkillFishPercent = 0,
                    Experience = 15694800,
                    Level = 100,
                    LevelPercent = 0,
                    Mana = 2850,
                    MaxMana = 2850,
                    Soul = 100,
                    Capacity = 1390 * 100,
                    Stamina = 42 * 60,
                    Gender = (int)Gender.Male,
                    Vocation = (int)Vocation.Sorcerer,
                    CoordinateX = 930, 
                    CoordinateY = 779, 
                    CoordinateZ = 7,
                    PlayerItems = new List<PlayerItem>()
                    {
                        new PlayerItem() { SequenceId = 101, ParentId = 3, OpenTibiaId = 1987, Count = 1 },
                        new PlayerItem() { SequenceId = 102, ParentId = 101, OpenTibiaId = 2120, Count = 1 },
                        new PlayerItem() { SequenceId = 103, ParentId = 101, OpenTibiaId = 2554, Count = 1 }
                    }
                } );
            }

            base.Seed(context);
        }
    }
}