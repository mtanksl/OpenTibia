using OpenTibia.Data.Contexts;
using OpenTibia.Data.Models;
using OpenTibia.Data.Repositories;
using System;
using System.Threading.Tasks;

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

        private IAccountRepository accountRepository;

        /// <exception cref="ObjectDisposedException"></exception>

        public IAccountRepository AccountRepository
        {
            get
            {
                if (disposed)
                {
                    throw new ObjectDisposedException(nameof(Database) );
                }

                return accountRepository ?? (accountRepository = new AccountRepository(databaseContext) );
            }
        }

        private IBanRepository banRepository;

        /// <exception cref="ObjectDisposedException"></exception>

        public IBanRepository BanRepository
        {
            get
            {
                if (disposed)
                {
                    throw new ObjectDisposedException(nameof(Database) );
                }

                return banRepository ?? (banRepository = new BanRepository(databaseContext) );
            }
        }

        private IBugReportRepository bugReportRepository;

        /// <exception cref="ObjectDisposedException"></exception>

        public IBugReportRepository BugReportRepository
        {
            get
            {
                if (disposed)
                {
                    throw new ObjectDisposedException(nameof(Database) );
                }

                return bugReportRepository ?? (bugReportRepository = new BugReportRepository(databaseContext) );
            }
        }

        private IDebugAssertRepository debugAssertRepository;

        /// <exception cref="ObjectDisposedException"></exception>

        public IDebugAssertRepository DebugAssertRepository
        {
            get
            {
                if (disposed)
                {
                    throw new ObjectDisposedException(nameof(Database) );
                }

                return debugAssertRepository ?? (debugAssertRepository = new DebugAssertRepository(databaseContext) );
            }
        }

        private IHouseRepository houseRepository;

        /// <exception cref="ObjectDisposedException"></exception>

        public IHouseRepository HouseRepository
        {
            get
            {
                if (disposed)
                {
                    throw new ObjectDisposedException(nameof(Database) );
                }

                return houseRepository ?? (houseRepository = new HouseRepository(databaseContext) );
            }
        }

        private IRuleViolationReportRepository ruleViolationReportRepository;

        /// <exception cref="ObjectDisposedException"></exception>

        public IRuleViolationReportRepository RuleViolationReportRepository
        {
            get
            {
                if (disposed)
                {
                    throw new ObjectDisposedException(nameof(Database) );
                }

                return ruleViolationReportRepository ?? (ruleViolationReportRepository = new RuleViolationReportRepository(databaseContext) );
            }
        }

        private IPlayerRepository playerRepository;

        /// <exception cref="ObjectDisposedException"></exception>

        public IPlayerRepository PlayerRepository
        {
            get
            {
                if (disposed)
                {
                    throw new ObjectDisposedException(nameof(Database) );
                }

                return playerRepository ?? (playerRepository = new PlayerRepository(databaseContext) );
            }
        }

        private IMotdRepository motdRepository;

        /// <exception cref="ObjectDisposedException"></exception>

        public IMotdRepository MotdRepository
        {
            get
            {
                if (disposed)
                {
                    throw new ObjectDisposedException(nameof(Database) );
                }

                return motdRepository ?? (motdRepository = new MotdRepository(databaseContext) );
            }
        }

        private IServerStorageRepository serverStorageRepository;

        /// <exception cref="ObjectDisposedException"></exception>

        public IServerStorageRepository ServerStorageRepository
        {
            get
            {
                if (disposed)
                {
                    throw new ObjectDisposedException(nameof(Database));
                }

                return serverStorageRepository ?? (serverStorageRepository = new ServerStorageRepository(databaseContext));
            }
        }

        private IWorldRepository worldRepository;

        /// <exception cref="ObjectDisposedException"></exception>

        public IWorldRepository WorldRepository
        {
            get
            {
                if (disposed)
                {
                    throw new ObjectDisposedException(nameof(Database) );
                }

                return worldRepository ?? (worldRepository = new WorldRepository(databaseContext) );
            }
        }

        /// <exception cref="ObjectDisposedException"></exception>

        public bool CanConnect()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(Database) );
            }

            return databaseContext.Database.CanConnect();
        }

        /// <exception cref="ObjectDisposedException"></exception>

        public async Task CreateDatabase(int gamePort)
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(Database) );
            }

            await Task.Yield();

            await databaseContext.Database.EnsureDeletedAsync();

            databaseContext.Accounts.Add(new DbAccount() { Id = 1, Name = "1", Password = "1", PremiumUntil = null } );

            databaseContext.Worlds.Add(new DbWorld() { Id = 1, Name = "Cormaya", Ip = "127.0.0.1", Port = gamePort } );

            databaseContext.Players.Add(new DbPlayer() { Id = 1, AccountId = 1, WorldId = 1, Name = "Gamemaster", Health = 645, MaxHealth = 645, Direction = 2, BaseOutfitId = 266, OutfitId = 266, BaseSpeed = 2218, Speed = 2218, SkillMagicLevel = 0, SkillFist = 10, SkillClub = 10, SkillSword = 10, SkillAxe = 10, SkillDistance = 10, SkillShield = 10, SkillFish = 10, Experience = 15694800, Level = 100, Mana = 550, MaxMana = 550, Soul = 100, Capacity = 139000, Stamina = 2520, Rank = 2, SpawnX = 921, SpawnY = 771, SpawnZ = 6, TownX = 921, TownY = 771, TownZ = 6 } );

            databaseContext.Players.Add(new DbPlayer() { Id = 2, AccountId = 1, WorldId = 1, Name = "Knight", Health = 1565, MaxHealth = 1565, Direction = 2, BaseOutfitId = 131, OutfitId = 131, BaseSpeed = 418, Speed = 418, SkillMagicLevel = 0, SkillFist = 10, SkillClub = 10, SkillSword = 90, SkillAxe = 10, SkillDistance = 10, SkillShield = 80, SkillFish = 10, Experience = 15694800, Level = 100, Mana = 550, MaxMana = 550, Soul = 100, Capacity = 139000, Stamina = 2520, Vocation = 1, SpawnX = 921, SpawnY = 771, SpawnZ = 6, TownX = 921, TownY = 771, TownZ = 6 } );

            databaseContext.Players.Add(new DbPlayer() { Id = 3, AccountId = 1, WorldId = 1, Name = "Paladin", Health = 1105, MaxHealth = 1105, Direction = 2, BaseOutfitId = 129, OutfitId = 129, BaseSpeed = 418, Speed = 418, SkillMagicLevel = 0, SkillFist = 10, SkillClub = 10, SkillSword = 10, SkillAxe = 10, SkillDistance = 70, SkillShield = 40, SkillFish = 10, Experience = 15694800, Level = 100, Mana = 1470, MaxMana = 1470, Soul = 100, Capacity = 139000, Stamina = 2520, Vocation = 2, SpawnX = 921, SpawnY = 771, SpawnZ = 6, TownX = 921, TownY = 771, TownZ = 6 } );

            databaseContext.Players.Add(new DbPlayer() { Id = 4, AccountId = 1, WorldId = 1, Name = "Sorcerer", Health = 645, MaxHealth = 645, Direction = 2, BaseOutfitId = 130, OutfitId = 130, BaseSpeed = 418, Speed = 418, SkillMagicLevel = 0, SkillFist = 10, SkillClub = 10, SkillSword = 10, SkillAxe = 10, SkillDistance = 10, SkillShield = 10, SkillFish = 10, Experience = 15694800, Level = 100, Mana = 2850, MaxMana = 2850, Soul = 100, Capacity = 139000, Stamina = 2520, Vocation = 4, SpawnX = 921, SpawnY = 771, SpawnZ = 6, TownX = 921, TownY = 771, TownZ = 6 } );

            databaseContext.Players.Add(new DbPlayer() { Id = 5, AccountId = 1, WorldId = 1, Name = "Druid", Health = 645, MaxHealth = 645, Direction = 2, BaseOutfitId = 130, OutfitId = 130, BaseSpeed = 418, Speed = 418, SkillMagicLevel = 0, SkillFist = 10, SkillClub = 10, SkillSword = 10, SkillAxe = 10, SkillDistance = 10, SkillShield = 10, SkillFish = 10, Experience = 15694800, Level = 100, Mana = 2850, MaxMana = 2850, Soul = 100, Capacity = 139000, Stamina = 2520, Vocation = 3, SpawnX = 921, SpawnY = 771, SpawnZ = 6, TownX = 921, TownY = 771, TownZ = 6 } );

            await databaseContext.SaveChangesAsync();
        }

        /// <exception cref="ObjectDisposedException"></exception>

        public async Task Commit()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(Database) );
            }

            await Task.Yield();    

            await databaseContext.SaveChangesAsync();
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