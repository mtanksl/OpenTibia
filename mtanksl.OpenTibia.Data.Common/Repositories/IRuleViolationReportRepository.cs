using OpenTibia.Data.Models;

namespace OpenTibia.Data.Repositories
{
    public interface IRuleViolationReportRepository
    {
        void AddRuleViolationReport(DbRuleViolationReport ruleViolationReport);
    }
}