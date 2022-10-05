namespace OpenTibia.Data.Migrations
{
    using OpenTibia.Data.Contexts;
    using OpenTibia.Data.Models;
    using System.Data.Entity.Migrations;
    using System.Data.SQLite.EF6.Migrations;

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
            var world = new World() { Id = 1, Name = "World", Ip = "127.0.0.1", Port = 7172 };

            context.Worlds.AddOrUpdate(world);

            var account = new Account() { Id = 1, Name = "1", Password = "1", PremiumDays = 0 };

            context.Accounts.AddOrUpdate(account);

            context.Players.AddOrUpdate(new Player() { Id = 1, AccountId = account.Id, WorldId = world.Id, Name = "Player 1", CoordinateX = 930, CoordinateY = 779, CoordinateZ = 7 } );

            context.Players.AddOrUpdate(new Player() { Id = 2, AccountId = account.Id, WorldId = world.Id, Name = "Player 2", CoordinateX = 931, CoordinateY = 779, CoordinateZ = 7 } );

            context.Players.AddOrUpdate(new Player() { Id = 3, AccountId = account.Id, WorldId = world.Id, Name = "Player 3", CoordinateX = 932, CoordinateY = 779, CoordinateZ = 7 } );

            base.Seed(context);
        }
    }
}