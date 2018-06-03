using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using OpenTibia.Web;

namespace OpenTibia.Game.Commands
{
    public class CreateReportRuleViolationCommand : Command
    {
        private Server server;

        public CreateReportRuleViolationCommand(Server server)
        {
            this.server = server;
        }

        public Player Player { get; set; }

        public string Message { get; set; }

        public override void Execute(Context context)
        {
            //Arrange

            RuleViolation ruleViolation = server.RuleViolations.GetRuleViolation(Player);

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
                    context.Response.Write(observer.Client.Connection, new ShowText(0, ruleViolation.Reporter.Name, ruleViolation.Reporter.Level, TalkType.ReportRuleViolationOpen, ruleViolation.Time, ruleViolation.Message) );
                }
            }
        }
    }
}