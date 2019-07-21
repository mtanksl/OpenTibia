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

            Player reporter = server.Map.GetPlayers()
                .Where(p => p.Name == Name)
                .FirstOrDefault();

            if (reporter != null)
            {
                RuleViolation ruleViolation = server.RuleViolations.GetRuleViolationByReporter(reporter);

                if (ruleViolation != null)
                {
                    if (ruleViolation.Assignee == null)
                    {
                        //Act

                        server.RuleViolations.RemoveRuleViolation(ruleViolation);

                        //Notify

                        foreach (var observer in server.Channels.GetChannel(3).GetPlayers() )
                        {
                            context.Write(observer.Client.Connection, new RemoveRuleViolationOutgoingPacket(ruleViolation.Reporter.Name) );
                        }

                        context.Write(ruleViolation.Reporter.Client.Connection, new CloseRuleViolationOutgoingPacket() );

                        base.Execute(server, context);
                    }
                    else if (ruleViolation.Assignee == Player)
                    {
                        //Act

                        server.RuleViolations.RemoveRuleViolation(ruleViolation);

                        //Notify

                        context.Write(ruleViolation.Reporter.Client.Connection, new CloseRuleViolationOutgoingPacket() );

                        base.Execute(server, context);
                    }
                }
            }
        }
    }
}