using OpenTibia.Data.Contexts;
using OpenTibia.Data.Repositories;
using System;

namespace OpenTibia.Game
{
    public class DatabaseContext : IDisposable
    {
        public DatabaseContext()
        {

        }

        ~DatabaseContext()
        {
            Dispose(false);
        }

        private SqliteContext sqliteContext;

        public SqliteContext SqliteContext
        {
            get
            {
                return sqliteContext ?? (sqliteContext = new SqliteContext() );
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