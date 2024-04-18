using OpenTibia.Data.Contexts;
using OpenTibia.Data.Models;

namespace OpenTibia.Data.Repositories
{
    public class BugReportRepository : IBugReportRepository
    {
        private DatabaseContext context;

        public BugReportRepository(DatabaseContext context)
        {
            this.context = context;
        }

        public void AddBugReport(DbBugReport bugReport)
        {
            context.BugReports.Add(bugReport);
        }
    }
}