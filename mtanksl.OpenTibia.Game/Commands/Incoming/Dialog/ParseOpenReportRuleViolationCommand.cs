using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ParseOpenReportRuleViolationCommand : IncomingCommand
    {
        public ParseOpenReportRuleViolationCommand(Player player, string message)
        {
            Player = player;

            Message = message;
        }

        public Player Player { get; set; }

        public string Message { get; set; }

        public override Promise Execute()
        {
            // ctrl + R

            RuleViolation ruleViolation = Context.Server.RuleViolations.GetRuleViolationByReporter(Player);

            if (ruleViolation == null)
            {
                ruleViolation = new RuleViolation()
                {
                    Reporter = Player,

                    Message = Message
                };

                Context.Server.RuleViolations.AddRuleViolation(ruleViolation);

                foreach (var observer in Context.Server.Channels.GetChannel(3).GetMembers() )
                {
                    Context.AddPacket(observer, new ShowTextOutgoingPacket(0, ruleViolation.Reporter.Name, ruleViolation.Reporter.Level, TalkType.ReportRuleViolationOpen, ruleViolation.Time, ruleViolation.Message) );
                }

                return Promise.Completed;
            }

            return Promise.Break;
        }
    }
}