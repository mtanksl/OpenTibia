using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Common.Objects
{
    public class RuleViolationCollection
    {
        private List<RuleViolation> ruleViolations = new List<RuleViolation>();

        public void AddRuleViolation(RuleViolation ruleViolation)
        {
            ruleViolations.Add(ruleViolation);
        }

        public void RemoveRuleViolation(RuleViolation ruleViolation)
        {
            ruleViolations.Remove(ruleViolation);
        }
        
        public RuleViolation GetRuleViolationByReporter(Player reporter)
        {
            return GetRuleViolations()
                .Where(r => r.Reporter == reporter)
                .FirstOrDefault();
        }
        
        public IEnumerable<RuleViolation> GetRuleViolations()
        {
            return ruleViolations;
        }
    }
}