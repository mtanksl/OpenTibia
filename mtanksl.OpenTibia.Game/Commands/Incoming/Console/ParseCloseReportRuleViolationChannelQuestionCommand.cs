using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ParseCloseReportRuleViolationChannelQuestionCommand : Command
    {
        public ParseCloseReportRuleViolationChannelQuestionCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        public override Promise Execute()
        {
            return Promise.Run( (resolve, reject) =>
            {
                RuleViolation ruleViolation = context.Server.RuleViolations.GetRuleViolationByReporter(Player);

                if (ruleViolation != null)
                {
                    if (ruleViolation.Assignee == null)
                    {
                        context.Server.RuleViolations.RemoveRuleViolation(ruleViolation);

                        foreach (var observer in context.Server.Channels.GetChannel(3).GetPlayers() )
                        {
                            context.AddPacket(observer.Client.Connection, new RemoveRuleViolationOutgoingPacket(ruleViolation.Reporter.Name) );
                        }

                        resolve(context);
                    }
                    else
                    {
                        context.Server.RuleViolations.RemoveRuleViolation(ruleViolation);

                        context.AddPacket(ruleViolation.Assignee.Client.Connection, new CancelRuleViolationOutgoingPacket(ruleViolation.Reporter.Name) );

                        resolve(context);
                    }
                }
            } );            
        }
    }
}