using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using OpenTibia.Web;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class AnswerInReportRuleViolationChannelCommand : Command
    {
        private Server server;

        public AnswerInReportRuleViolationChannelCommand(Server server)
        {
            this.server = server;
        }

        public Player Player { get; set; }

        public string Name { get; set; }

        public string Message { get; set; }

        public override void Execute(Context context)
        {
            //Arrange

            Player observer = server.Map.GetPlayers().Where(p => p.Name == Name).FirstOrDefault();

            //Act
            
            if (observer != null)
            {
                RuleViolation ruleViolation = server.RuleViolations.GetRuleViolation(observer);

                if (ruleViolation != null)
                {
                    if (ruleViolation.Assignee == Player)
                    {
                        //Notify

                        context.Response.Write(ruleViolation.Reporter.Client.Connection, new ShowText(0, ruleViolation.Assignee.Name, ruleViolation.Assignee.Level, TalkType.ReportRuleViolationAnswer, Message) );
                    }
                }
            }
        }
    }
}