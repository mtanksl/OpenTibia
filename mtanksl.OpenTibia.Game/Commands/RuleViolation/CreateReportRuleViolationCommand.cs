using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class CreateReportRuleViolationCommand : Command
    {
        public CreateReportRuleViolationCommand(Player player, string message)
        {
            Player = player;

            Message = message;
        }

        public Player Player { get; set; }

        public string Message { get; set; }

        public override void Execute(Context context)
        {
            //Arrange

            RuleViolation ruleViolation = context.Server.RuleViolations.GetRuleViolationByReporter(Player);

            if (ruleViolation == null)
            {
                //Act

                ruleViolation = new RuleViolation()
                {
                    Reporter = Player,

                    Message = Message
                };

                context.Server.RuleViolations.AddRuleViolation(ruleViolation);

                //Notify

                foreach (var observer in context.Server.Channels.GetChannel(3).GetPlayers() )
                {
                    context.AddPacket(observer.Client.Connection, new ShowTextOutgoingPacket(0, ruleViolation.Reporter.Name, ruleViolation.Reporter.Level, TalkType.ReportRuleViolationOpen, ruleViolation.Time, ruleViolation.Message) );
                }

                base.Execute(context);
            }
        }
    }
}