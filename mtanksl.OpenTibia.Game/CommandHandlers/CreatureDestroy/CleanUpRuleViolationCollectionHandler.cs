using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
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
                    foreach (var ruleViolation in Context.Server.RuleViolations.GetRuleViolations().ToArray() )
                    {
                        if (ruleViolation.Reporter == player)
                        {
                            if (ruleViolation.Assignee == null)
                            {
                                Context.Server.RuleViolations.RemoveRuleViolation(ruleViolation);

                                var ruleViolationChannel = Context.Server.Channels.GetChannels()
                                    .Where(c => c.Flags.Is(ChannelFlags.RuleViolations) )
                                    .FirstOrDefault();

                                foreach (var observer in ruleViolationChannel.GetMembers() )
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