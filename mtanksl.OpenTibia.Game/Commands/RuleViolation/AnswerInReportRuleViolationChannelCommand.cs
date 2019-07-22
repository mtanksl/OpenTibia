using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class AnswerInReportRuleViolationChannelCommand : Command
    {
        public AnswerInReportRuleViolationChannelCommand(Player player, string name, string message)
        {
            Player = player;

            Name = name;

            Message = message;
        }

        public Player Player { get; set; }

        public string Name { get; set; }

        public string Message { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Player reporter = server.Map.GetPlayers()
                .Where(p => p.Name == Name)
                .FirstOrDefault();

            if (reporter != null)
            {
                RuleViolation ruleViolation = server.RuleViolations.GetRuleViolationByReporter(reporter);

                if (ruleViolation != null && ruleViolation.Assignee == Player)
                {
                    //Act

                    //Notify

                    context.Write(ruleViolation.Reporter.Client.Connection, new ShowTextOutgoingPacket(0, ruleViolation.Assignee.Name, ruleViolation.Assignee.Level, TalkType.ReportRuleViolationAnswer, Message) );

                    base.Execute(server, context);
                }
            }
        }
    }
}