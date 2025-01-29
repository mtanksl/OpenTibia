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

        public async Task CreateInMemoryDatabase()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(Database) );
            }

            await Task.Yield();

            await databaseContext.Database.EnsureDeletedAsync();

            databaseContext.Seed();

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