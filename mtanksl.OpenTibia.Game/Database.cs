using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using OpenTibia.Data.Contexts;
using OpenTibia.Data.Repositories;
using System;

namespace OpenTibia.Game
{
    public class Database : IDisposable
    {
        public Database(Server server)
        {
            this.server = server;
        }

        ~Database()
        {
            Dispose(false);
        }

        private Server server;

        public Server Server
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
                if (databaseContext == null)
                {
                    var builder = new DbContextOptionsBuilder<SqliteContext>();

                    builder.LogTo(

                        action:                         
                            message => server.Logger.WriteLine(message.Substring(message.IndexOf("CommandType='Text', CommandTimeout='30'") + 40), LogLevel.Debug),

                        events: 
                            new[] { RelationalEventId.CommandExecuted }, 

                        options:                         
                            DbContextLoggerOptions.SingleLine
                    );

                    databaseContext = new SqliteContext(server.PathResolver.GetFullPath("data/database.db"), builder.Options);
                }

                return databaseContext;
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