using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class AskInReportRuleViolationChannelCommand : Command
    {
        public AskInReportRuleViolationChannelCommand(Player player, string message)
        {
            Player = player;

            Message = message;
        }

        public Player Player { get; set; }

        public string Message { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            RuleViolation ruleViolation = server.RuleViolations.GetRuleViolationByReporter(Player);

            //Act

            //Notify

            if (ruleViolation != null && ruleViolation.Assignee != null)
            {
                context.Write(ruleViolation.Assignee.Client.Connection, new ShowTextOutgoingPacket(0, ruleViolation.Reporter.Name, ruleViolation.Reporter.Level, TalkType.ReportRuleViolationQuestion, Message) );

                base.Execute(server, context);
            }
        }
    }
}