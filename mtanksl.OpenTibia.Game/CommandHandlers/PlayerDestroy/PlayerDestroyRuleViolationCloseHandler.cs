using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class PlayerDestroyRuleViolationCloseHandler : CommandHandler<PlayerDestroyCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerDestroyCommand command)
        {
            return next().Then( () =>
            {
                foreach (var ruleViolation in Context.Server.RuleViolations.GetRuleViolations() )
                {
                    if (ruleViolation.Reporter == command.Player)
                    {
                       //TODO
                    }

                    if (ruleViolation.Assignee == command.Player)
                    {
                        //TODO
                    }
                }

                return Promise.Completed;
            } );
        }
    }
}