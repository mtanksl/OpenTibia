using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class CloseReportRuleViolationChannelQuestionCommand : Command
    {
        public CloseReportRuleViolationChannelQuestionCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange
            
            RuleViolation ruleViolation = server.RuleViolations.GetRuleViolation(Player);

            if (ruleViolation != null)
            {
                if (ruleViolation.Assignee == null)
                {
                    server.RuleViolations.RemoveRuleViolation(ruleViolation);

                    //Notify

                    foreach (var observer2 in server.Channels.GetChannel(3).GetPlayers() )
                    {
                        context.Write(observer2.Client.Connection, new RemoveRuleViolationOutgoingPacket(ruleViolation.Reporter.Name) );
                    }
                }
                else
                {
                    server.RuleViolations.RemoveRuleViolation(ruleViolation);

                    //Notify

                    context.Write(ruleViolation.Assignee.Client.Connection, new CancelRuleViolationOutgoingPacket(ruleViolation.Reporter.Name) );
                }
            }

            base.Execute(server, context);
        }
    }
}