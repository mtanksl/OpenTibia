using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;
using OpenTibia.Web;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class ProcessReportRuleViolationCommand : Command
    {
        private Server server;

        public ProcessReportRuleViolationCommand(Server server)
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
                        ruleViolation.Assignee = Player;

                        //Notify

                        foreach (var observer2 in server.Channels.GetChannel(3).GetPlayers() )
                        {
                            context.Response.Write(observer2.Client.Connection, new RemoveRuleViolation(ruleViolation.Reporter.Name) );
                        }
                    }
                }
            }
        }
    }
}