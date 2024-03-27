using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Linq;

namespace OpenTibia.Game.CommandHandlers
{
    public class CleanUpRuleViolationCollectionHandler : CommandHandler<CreatureDestroyCommand>
    {
        public override Promise Handle(Func<Promise> next, CreatureDestroyCommand command)
        {
            if (command.Creature is Player player)
            {
                return next().Then( () =>
                {
                    foreach (var ruleViolation in Context.Server.RuleViolations.GetRuleViolations().ToList() )
                    {
                        if (ruleViolation.Reporter == player)
                        {
                            if (ruleViolation.Assignee == null)
                            {
                                Context.Server.RuleViolations.RemoveRuleViolation(ruleViolation);

                                foreach (var observer in Context.Server.Channels.GetChannel(3).GetMembers() )
                                {
                                    Context.AddPacket(observer, new RemoveRuleViolationOutgoingPacket(ruleViolation.Reporter.Name) );
                                }
                            }
                            else
                            {
                                Context.Server.RuleViolations.RemoveRuleViolation(ruleViolation);

                                Context.AddPacket(ruleViolation.Assignee, new CancelRuleViolationOutgoingPacket(ruleViolation.Reporter.Name) );
                            }
                        }
                        else if (ruleViolation.Assignee == player)
                        {
                            Context.Server.RuleViolations.RemoveRuleViolation(ruleViolation);

                            Context.AddPacket(ruleViolation.Reporter, new CloseRuleViolationOutgoingPacket() );
                        }
                    }

                    return Promise.Completed;
                } );
            }

            return next();
        }
    }
}