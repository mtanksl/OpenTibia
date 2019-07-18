using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class CloseReportRuleViolationChannelAnswerCommand : Command
    {
        public CloseReportRuleViolationChannelAnswerCommand(Player player, string name)
        {
            Player = player;

            Name = name;
        }

        public Player Player { get; set; }

        public string Name { get; set; }

        public override void Execute(Server server, CommandContext context)
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
                            context.Write(observer2.Client.Connection, new RemoveRuleViolation(ruleViolation.Reporter.Name) );
                        }

                        context.Write(ruleViolation.Reporter.Client.Connection, new CloseRuleViolation() );
                    }
                    else if (ruleViolation.Assignee == Player)
                    {
                        server.RuleViolations.RemoveRuleViolation(ruleViolation);

                        //Notify

                        context.Write(ruleViolation.Reporter.Client.Connection, new CloseRuleViolation() );
                    }
                }
            }
        }
    }
}