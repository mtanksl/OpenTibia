using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class AnswerReportRuleViolationCommand : Command
    {
        public AnswerReportRuleViolationCommand(Player player, string name, string message)
        {
            Player = player;

            Name = name;

            Message = message;
        }

        public Player Player { get; set; }

        public string Name { get; set; }

        public string Message { get; set; }

        public override void Execute(Context context)
        {
            Player reporter = context.Server.GameObjects.GetPlayers()
                .Where(p => p.Name == Name)
                .FirstOrDefault();

            if (reporter != null)
            {
                RuleViolation ruleViolation = context.Server.RuleViolations.GetRuleViolationByReporter(reporter);

                if (ruleViolation != null && ruleViolation.Assignee == Player)
                {
                    context.AddPacket(ruleViolation.Reporter.Client.Connection, new ShowTextOutgoingPacket(0, ruleViolation.Assignee.Name, ruleViolation.Assignee.Level, TalkType.ReportRuleViolationAnswer, Message) );

                    base.Execute(context);
                }
            }
        }
    }
}