using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class QuestionReportRuleViolationCommand : Command
    {
        public QuestionReportRuleViolationCommand(Player player, string message)
        {
            Player = player;

            Message = message;
        }

        public Player Player { get; set; }

        public string Message { get; set; }

        public override void Execute(Context context)
        {
            RuleViolation ruleViolation = context.Server.RuleViolations.GetRuleViolationByReporter(Player);

            if (ruleViolation != null && ruleViolation.Assignee != null)
            {
                context.AddPacket(ruleViolation.Assignee.Client.Connection, new ShowTextOutgoingPacket(0, ruleViolation.Reporter.Name, ruleViolation.Reporter.Level, TalkType.ReportRuleViolationQuestion, Message) );

                OnComplete(context);
            }
        }
    }
}