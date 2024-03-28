using OpenTibia.Data.Contexts;
using OpenTibia.Data.Repositories;
using System;

namespace OpenTibia.Game
{
    public class Database : IDisposable
    {
        public Database(IServer server)
        {
            this.server = server;
        }

        ~Database()
        {
            Dispose(false);
        }

        private IServer server;

        public IServer Server
        {
            get
            {
                return server;
            }
        }

        private DatabaseContext databaseContext;

        public DatabaseContext DatabaseContext
        {
            get
            {
                return databaseContext ?? (databaseContext = server.DatabaseFactory.Create() );
            }
        }

        private BanRepository banRepository;

        public BanRepository BanRepository
        {
            get
            {
                return banRepository ?? (banRepository = new BanRepository(DatabaseContext) );
            }
        }

        private BugReportRepository bugReportRepository;

        public BugReportRepository BugReportRepository
        {
            get
            {
                return bugReportRepository ?? (bugReportRepository = new BugReportRepository(DatabaseContext) );
            }
        }

        private DebugAssertRepository debugAssertRepository;

        public DebugAssertRepository DebugAssertRepository
        {
            get
            {
                return debugAssertRepository ?? (debugAssertRepository = new DebugAssertRepository(DatabaseContext) );
            }
        }

        private RuleViolationReportRepository ruleViolationReportRepository;

        public RuleViolationReportRepository RuleViolationReportRepository
        {
            get
            {
                return ruleViolationReportRepository ?? (ruleViolationReportRepository = new RuleViolationReportRepository(DatabaseContext) );
            }
        }

        private PlayerRepository playerRepository;

        public PlayerRepository PlayerRepository
        {
            get
            {
                return playerRepository ?? (playerRepository = new PlayerRepository(DatabaseContext) );
            }
        }

        private MotdRepository motdRepository;

        public MotdRepository MotdRepository
        {
            get
            {
                return motdRepository ?? (motdRepository = new MotdRepository(DatabaseContext) );
            }
        }

        public void Commit()
        {
            DatabaseContext.SaveChanges();
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