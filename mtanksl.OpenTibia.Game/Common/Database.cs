using OpenTibia.Data.Contexts;
using OpenTibia.Data.Models;
using OpenTibia.Data.Repositories;
using System;

namespace OpenTibia.Game.Common
{
    public class Database : IDatabase
    {
        private DatabaseContext databaseContext;

        public Database(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        ~Database()
        {
            Dispose(false);
        }

        private IBanRepository banRepository;

        public IBanRepository BanRepository
        {
            get
            {
                return banRepository ?? (banRepository = new BanRepository(databaseContext) );
            }
        }

        private IBugReportRepository bugReportRepository;

        public IBugReportRepository BugReportRepository
        {
            get
            {
                return bugReportRepository ?? (bugReportRepository = new BugReportRepository(databaseContext) );
            }
        }

        private IDebugAssertRepository debugAssertRepository;

        public IDebugAssertRepository DebugAssertRepository
        {
            get
            {
                return debugAssertRepository ?? (debugAssertRepository = new DebugAssertRepository(databaseContext) );
            }
        }

        private IHouseRepository houseRepository;

        public IHouseRepository HouseRepository
        {
            get
            {
                return houseRepository ?? (houseRepository = new HouseRepository(databaseContext) );
            }
        }

        private IRuleViolationReportRepository ruleViolationReportRepository;

        public IRuleViolationReportRepository RuleViolationReportRepository
        {
            get
            {
                return ruleViolationReportRepository ?? (ruleViolationReportRepository = new RuleViolationReportRepository(databaseContext) );
            }
        }

        private IPlayerRepository playerRepository;

        public IPlayerRepository PlayerRepository
        {
            get
            {
                return playerRepository ?? (playerRepository = new PlayerRepository(databaseContext) );
            }
        }

        private IMotdRepository motdRepository;

        public IMotdRepository MotdRepository
        {
            get
            {
                return motdRepository ?? (motdRepository = new MotdRepository(databaseContext) );
            }
        }

        public bool CanConnect()
        {
            return databaseContext.Database.CanConnect();
        }

        public void CreateDatabase(int gamePort)
        {
            databaseContext.Database.EnsureDeleted();

            databaseContext.Accounts.Add(new DbAccount() { Id = 1, Name = "1", Password = "1", PremiumDays = 0 } );

            databaseContext.Worlds.Add(new DbWorld() { Id = 1, Name = "Cormaya", Ip = "127.0.0.1", Port = gamePort } );

            databaseContext.Players.Add(new DbPlayer() { Id = 1, AccountId = 1, WorldId = 1, Name = "Gamemaster", Health = 645, MaxHealth = 645, Direction = 2, BaseOutfitId = 266, OutfitId = 266, BaseSpeed = 2218, Speed = 2218, Experience = 15694800, Level = 100, Mana = 550, MaxMana = 550, Soul = 100, Capacity = 139000, Stamina = 2520, Rank = 2, SpawnX = 921, SpawnY = 771, SpawnZ = 6, TownX = 921, TownY = 771, TownZ = 6 } );

            databaseContext.Players.Add(new DbPlayer() { Id = 2, AccountId = 1, WorldId = 1, Name = "Knight", Health = 1565, MaxHealth = 1565, Direction = 2, BaseOutfitId = 131, OutfitId = 131, BaseSpeed = 418, Speed = 418, Experience = 15694800, Level = 100, Mana = 550, MaxMana = 550, Soul = 100, Capacity = 139000, Stamina = 2520, Vocation = 1, SpawnX = 921, SpawnY = 771, SpawnZ = 6, TownX = 921, TownY = 771, TownZ = 6 } );

            databaseContext.Players.Add(new DbPlayer() { Id = 3, AccountId = 1, WorldId = 1, Name = "Paladin", Health = 1105, MaxHealth = 1105, Direction = 2, BaseOutfitId = 129, OutfitId = 129, BaseSpeed = 418, Speed = 418, Experience = 15694800, Level = 100, Mana = 1470, MaxMana = 1470, Soul = 100, Capacity = 139000, Stamina = 2520, Vocation = 2, SpawnX = 921, SpawnY = 771, SpawnZ = 6, TownX = 921, TownY = 771, TownZ = 6 } );

            databaseContext.Players.Add(new DbPlayer() { Id = 4, AccountId = 1, WorldId = 1, Name = "Sorcerer", Health = 645, MaxHealth = 645, Direction = 2, BaseOutfitId = 130, OutfitId = 130, BaseSpeed = 418, Speed = 418, Experience = 15694800, Level = 100, Mana = 2850, MaxMana = 2850, Soul = 100, Capacity = 139000, Stamina = 2520, Vocation = 4, SpawnX = 921, SpawnY = 771, SpawnZ = 6, TownX = 921, TownY = 771, TownZ = 6 });

            databaseContext.Players.Add(new DbPlayer() { Id = 5, AccountId = 1, WorldId = 1, Name = "Druid", Health = 645, MaxHealth = 645, Direction = 2, BaseOutfitId = 130, OutfitId = 130, BaseSpeed = 418, Speed = 418, Experience = 15694800, Level = 100, Mana = 2850, MaxMana = 2850, Soul = 100, Capacity = 139000, Stamina = 2520, Vocation = 3, SpawnX = 921, SpawnY = 771, SpawnZ = 6, TownX = 921, TownY = 771, TownZ = 6 } );

            databaseContext.SaveChanges();
        }

        public void Commit()
        {
            databaseContext.SaveChanges();
        }

        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if ( !disposed )
            {
                disposed = true;

                if (disposing)
                {
                    if (databaseContext != null)
                    {
                        databaseContext.Dispose();
                    }
                }
            }
        }
    }
}