using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;
using OpenTibia.Web;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class CloseReportRuleViolationChannelAnswerCommand : Command
    {
        private Server server;

        public CloseReportRuleViolationChannelAnswerCommand(Server server)
        {
            this.server = server;
        }

        public Player Player { get; set; }

        public string Name { get; set; }

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
                    if (ruleViolation.Assignee == null)
                    {
                        server.RuleViolations.RemoveRuleViolation(ruleViolation);

                        //Notify

                        foreach (var observer2 in server.Channels.GetChannel(3).GetPlayers() )
                        {
                            context.Response.Write(observer2.Client.Connection, new RemoveRuleViolation(ruleViolation.Reporter.Name) );
                        }

                        context.Response.Write(ruleViolation.Reporter.Client.Connection, new CloseRuleViolation() );
                    }
                    else if (ruleViolation.Assignee == Player)
                    {
                        server.RuleViolations.RemoveRuleViolation(ruleViolation);

                        //Notify

                        context.Response.Write(ruleViolation.Reporter.Client.Connection, new CloseRuleViolation() );
                    }
                }
            }
        }
    }
}