using OpenTibia.Data.Models;

namespace OpenTibia.Data.Repositories
{
    public interface IBugReportRepository
    {
        void AddBugReport(DbBugReport bugReport);
    }
}