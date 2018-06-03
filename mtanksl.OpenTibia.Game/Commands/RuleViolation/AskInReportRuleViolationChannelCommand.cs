using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using OpenTibia.Web;

namespace OpenTibia.Game.Commands
{
    public class AskInReportRuleViolationChannelCommand : Command
    {
        private Server server;

        public AskInReportRuleViolationChannelCommand(Server server)
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
            
            if (ruleViolation != null)
            {
                if (ruleViolation.Assignee != null)
                {
                    //Notify
                    
                    context.Response.Write(ruleViolation.Assignee.Client.Connection, new ShowText(0, ruleViolation.Reporter.Name, ruleViolation.Reporter.Level, TalkType.ReportRuleViolationQuestion, Message) );
                }
            }
        }
    }
}