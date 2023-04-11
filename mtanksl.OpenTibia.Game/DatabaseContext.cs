using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using OpenTibia.Data.Contexts;
using OpenTibia.Data.Repositories;
using System;

namespace OpenTibia.Game
{
    public class DatabaseContext : IDisposable
    {
        public DatabaseContext(Server server)
        {
            this.server = server;
        }

        ~DatabaseContext()
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

        private SqliteContext sqliteContext;

        public SqliteContext SqliteContext
        {
            get
            {
                if (sqliteContext == null)
                {
                    var builder = new DbContextOptionsBuilder<SqliteContext>();

                    builder.LogTo(message => server.Logger.WriteLine(message.Split("CommandTimeout='30']")[1], LogLevel.Information), 

                        events: new[] { RelationalEventId.CommandExecuted }, 

                        options: DbContextLoggerOptions.SingleLine );

                    sqliteContext = new SqliteContext(builder.Options);
                }

                return sqliteContext;
            }
        }

        private PlayerRepository playerRepository;

        public PlayerRepository PlayerRepository
        {
            get
            {
                return playerRepository ?? (playerRepository = new PlayerRepository(SqliteContext) );
            }
        }

        public void Commit()
        {
            SqliteContext.SaveChanges();
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
                    if (sqliteContext != null)
                    {
                        sqliteContext.Dispose();
                    }
                }
            }
        }
    }
}