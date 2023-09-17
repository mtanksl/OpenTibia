using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using OpenTibia.Data.Contexts;
using OpenTibia.Data.Repositories;
using System;
using System.IO;

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
                    if ( !server.PathResolver.Exists(server.Config.DatabaseFile) )
                    {
                        var template = server.PathResolver.GetFullPath("data/template.db");

                        var database = Path.Combine(Path.GetDirectoryName(template), Path.GetFileName(server.Config.DatabaseFile) );

                        File.Copy(template, database);
                    }

                    var builder = new DbContextOptionsBuilder<SqliteContext>();

                    builder.LogTo(

                        action:                         
                            message => server.Logger.WriteLine(message.Substring(message.IndexOf("CommandType='Text', CommandTimeout='30'") + 40), LogLevel.Debug),

                        events: 
                            new[] { RelationalEventId.CommandExecuted }, 

                        options:                         
                            DbContextLoggerOptions.SingleLine
                    );

                    databaseContext = new SqliteContext(server.PathResolver.GetFullPath(server.Config.DatabaseFile), builder.Options);
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