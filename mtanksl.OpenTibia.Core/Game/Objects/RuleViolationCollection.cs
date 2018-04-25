using System.Linq;
using System.Collections.Generic;

namespace OpenTibia
{
    public class RuleViolationCollection
    {
        private List<RuleViolation> ruleViolations = new List<RuleViolation>();

        public RuleViolation AddRuleViolation(RuleViolation ruleViolation)
        {
            ruleViolations.Add(ruleViolation);

            return ruleViolation;
        }

        public void RemoveRuleViolation(RuleViolation ruleViolation)
        {
            ruleViolations.Remove(ruleViolation);
        }
        
        public RuleViolation GetRuleViolation(Player reporter)
        {
            return GetRuleViolations().Where(ruleViolation => ruleViolation.Reporter == reporter).FirstOrDefault();
        }

        public IEnumerable<RuleViolation> GetRuleViolations()
        {
            return ruleViolations;
        }
    }
}