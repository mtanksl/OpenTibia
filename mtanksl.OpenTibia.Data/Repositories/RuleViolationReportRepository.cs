using OpenTibia.Data.Contexts;
using OpenTibia.Data.Models;

namespace OpenTibia.Data.Repositories
{
    public class RuleViolationReportRepository : IRuleViolationReportRepository
    {
        private DatabaseContext context;

        public RuleViolationReportRepository(DatabaseContext context)
        {
            this.context = context;
        }

        public void AddRuleViolationReport(DbRuleViolationReport ruleViolationReport)
        {
            context.RuleViolationReports.Add(ruleViolationReport);
        }
    }    
}