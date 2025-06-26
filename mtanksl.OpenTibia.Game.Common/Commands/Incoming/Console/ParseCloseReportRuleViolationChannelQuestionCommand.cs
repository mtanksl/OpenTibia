using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using OpenTibia.Network.Packets.Outgoing;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class ParseCloseReportRuleViolationChannelQuestionCommand : IncomingCommand
    {
        public ParseCloseReportRuleViolationChannelQuestionCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        public override Promise Execute()
        {
            RuleViolation ruleViolation = Context.Server.RuleViolations.GetRuleViolationByReporter(Player);

            if (ruleViolation != null)
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

                    return Promise.Completed;
                }
                else
                {
                    Context.Server.RuleViolations.RemoveRuleViolation(ruleViolation);

                    Context.AddPacket(ruleViolation.Assignee, new CancelRuleViolationOutgoingPacket(ruleViolation.Reporter.Name) );

                    return Promise.Completed;
                }
            }

            return Promise.Break;
        }
    }
}