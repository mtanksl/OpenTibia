using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class ProcessReportRuleViolationCommand : Command
    {
        public ProcessReportRuleViolationCommand(Player player, string name)
        {
            Player = player;

            Name = name;
        }

        public Player Player { get; set; }

        public string Name { get; set; }

        public override void Execute(Context context)
        {
            //Arrange

            Player reporter = context.Server.Map.GetPlayers()
                .Where(p => p.Name == Name)
                .FirstOrDefault();
            
            if (reporter != null)
            {
                RuleViolation ruleViolation = context.Server.RuleViolations.GetRuleViolationByReporter(reporter);

                if (ruleViolation != null && ruleViolation.Assignee == null)
                {
                    //Act

                    ruleViolation.Assignee = Player;

                    //Notify

                    foreach (var observer in context.Server.Channels.GetChannel(3).GetPlayers() )
                    {
                        context.AddPacket(observer.Client.Connection, new RemoveRuleViolationOutgoingPacket(ruleViolation.Reporter.Name) );
                    }

                    base.Execute(context);
                }
            }
        }
    }
}