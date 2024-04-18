using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using OpenTibia.Data.Contexts;
using System;

namespace OpenTibia.Game.Common.ServerObjects
{
    public class DatabaseFactory : IDatabaseFactory
    {
        private IServer server;

        private Func<DbContextOptionsBuilder, DatabaseContext> factory;

        public DatabaseFactory(IServer server, Func<DbContextOptionsBuilder, DatabaseContext> factory)
        {
            this.server = server;

            this.factory = factory;
        }

        public IDatabase Create()
        {
            var builder = new DbContextOptionsBuilder();

            builder.LogTo(

                action:                         
                    message => server.Logger.WriteLine(message.Substring(message.IndexOf("CommandType='Text', CommandTimeout='30'") + 40), LogLevel.Debug),

                events: 
                    new[] { RelationalEventId.CommandExecuted }, 

                options:                         
                    DbContextLoggerOptions.SingleLine
            );

            DatabaseContext databaseContext = factory(builder);

            return new Database(databaseContext);
        }
    }
}