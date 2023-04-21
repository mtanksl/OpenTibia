using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Linq;

namespace OpenTibia.Game.CommandHandlers
{
    public class PlayerDestroyRuleViolationCloseHandler : CommandHandler<PlayerDestroyCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerDestroyCommand command)
        {
            return next().Then( () =>
            {
                foreach (var ruleViolation in Context.Server.RuleViolations.GetRuleViolations().ToList() )
                {
                    if (ruleViolation.Reporter == command.Player)
                    {
                        if (ruleViolation.Assignee == null)
                        {
                            Context.Server.RuleViolations.RemoveRuleViolation(ruleViolation);

                            foreach (var observer in Context.Server.Channels.GetChannel(3).GetPlayers() )
                            {
                                Context.AddPacket(observer.Client.Connection, new RemoveRuleViolationOutgoingPacket(ruleViolation.Reporter.Name) );
                            }
                        }
                        else
                        {
                            Context.Server.RuleViolations.RemoveRuleViolation(ruleViolation);

                            Context.AddPacket(ruleViolation.Assignee.Client.Connection, new CancelRuleViolationOutgoingPacket(ruleViolation.Reporter.Name) );
                        }
                    }
                    else if (ruleViolation.Assignee == command.Player)
                    {
                        Context.Server.RuleViolations.RemoveRuleViolation(ruleViolation);

                        Context.AddPacket(ruleViolation.Reporter.Client.Connection, new CloseRuleViolationOutgoingPacket() );
                    }
                }

                return Promise.Completed;
            } );
        }
    }
}