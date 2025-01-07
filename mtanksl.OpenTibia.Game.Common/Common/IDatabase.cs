using OpenTibia.Data.Repositories;
using System;
using System.Threading.Tasks;

namespace OpenTibia.Game.Common
{
    public interface IDatabase : IDisposable
    {
        IAccountRepository AccountRepository { get; }

        IBanRepository BanRepository { get; }

        IBugReportRepository BugReportRepository { get; }

        IDebugAssertRepository DebugAssertRepository { get; }

        IHouseRepository HouseRepository { get; }

        IRuleViolationReportRepository RuleViolationReportRepository { get; }

        IPlayerRepository PlayerRepository { get; }

        IMotdRepository MotdRepository { get; }

        IServerStorageRepository ServerStorageRepository { get; }

        IWorldRepository WorldRepository { get; }

        bool CanConnect();

        Task CreateDatabase(int gamePort);

        Task Commit();
    }
}