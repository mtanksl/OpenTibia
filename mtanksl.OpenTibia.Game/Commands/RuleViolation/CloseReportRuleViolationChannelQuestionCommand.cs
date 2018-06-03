using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;
using OpenTibia.Web;

namespace OpenTibia.Game.Commands
{
    public class CloseReportRuleViolationChannelQuestionCommand : Command
    {
        private Server server;

        public CloseReportRuleViolationChannelQuestionCommand(Server server)
        {
            this.server = server;
        }

        public Player Player { get; set; }

        public override void Execute(Context context)
        {
            //Arrange
            
            RuleViolation ruleViolation = server.RuleViolations.GetRuleViolation(Player);

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
                }
                else
                {
                    server.RuleViolations.RemoveRuleViolation(ruleViolation);

                    //Notify

                    context.Response.Write(ruleViolation.Assignee.Client.Connection, new CancelRuleViolation(ruleViolation.Reporter.Name) );
                }
            }
        }
    }
}