using OpenTibia.Common.Objects;
using System.Collections.Generic;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface IRuleViolationCollection
    {
        void AddRuleViolation(RuleViolation ruleViolation);

        void RemoveRuleViolation(RuleViolation ruleViolation);

        RuleViolation GetRuleViolationByReporter(Player reporter);

        IEnumerable<RuleViolation> GetRuleViolations();
    }
}