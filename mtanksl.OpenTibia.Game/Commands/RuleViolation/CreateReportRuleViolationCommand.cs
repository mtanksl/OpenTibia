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

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            RuleViolation ruleViolation = server.RuleViolations.GetRuleViolationByReporter(Player);

            //Act

            if (ruleViolation == null)
            {
                ruleViolation = new RuleViolation()
                {
                    Reporter = Player,

                    Message = Message
                };

                server.RuleViolations.AddRuleViolation(ruleViolation);

                //Notify

                foreach (var observer in server.Channels.GetChannel(3).GetPlayers() )
                {
                    context.Write(observer.Client.Connection, new ShowTextOutgoingPacket(0, ruleViolation.Reporter.Name, ruleViolation.Reporter.Level, TalkType.ReportRuleViolationOpen, ruleViolation.Time, ruleViolation.Message) );
                }
            }

            base.Execute(server, context);
        }
    }
}