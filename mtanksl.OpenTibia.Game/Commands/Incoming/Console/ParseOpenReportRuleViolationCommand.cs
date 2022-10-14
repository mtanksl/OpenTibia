using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ParseOpenReportRuleViolationCommand : Command
    {
        public ParseOpenReportRuleViolationCommand(Player player, string message)
        {
            Player = player;

            Message = message;
        }

        public Player Player { get; set; }

        public string Message { get; set; }

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                RuleViolation ruleViolation = context.Server.RuleViolations.GetRuleViolationByReporter(Player);

                if (ruleViolation == null)
                {
                    ruleViolation = new RuleViolation()
                    {
                        Reporter = Player,

                        Message = Message
                    };

                    context.Server.RuleViolations.AddRuleViolation(ruleViolation);

                    foreach (var observer in context.Server.Channels.GetChannel(3).GetPlayers() )
                    {
                        context.AddPacket(observer.Client.Connection, new ShowTextOutgoingPacket(0, ruleViolation.Reporter.Name, ruleViolation.Reporter.Level, TalkType.ReportRuleViolationOpen, ruleViolation.Time, ruleViolation.Message) );
                    }

                    resolve(context);
                }
            } );
        }
    }
}