using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class ParseProcessReportRuleViolationCommand : Command
    {
        public ParseProcessReportRuleViolationCommand(Player player, string name)
        {
            Player = player;

            Name = name;
        }

        public Player Player { get; set; }

        public string Name { get; set; }

        public override Promise Execute()
        {
            if (Player.Vocation == Vocation.Gamemaster)
            {
                Player reporter = Context.Server.GameObjects.GetPlayers()
                    .Where(p => p.Name == Name)
                    .FirstOrDefault();
            
                if (reporter != null)
                {
                    RuleViolation ruleViolation = Context.Server.RuleViolations.GetRuleViolationByReporter(reporter);

                    if (ruleViolation != null && ruleViolation.Assignee == null)
                    {
                        ruleViolation.Assignee = Player;

                        foreach (var observer in Context.Server.Channels.GetChannel(3).GetPlayers() )
                        {
                            Context.AddPacket(observer.Client.Connection, new RemoveRuleViolationOutgoingPacket(ruleViolation.Reporter.Name) );
                        }

                        return Promise.Completed;
                    }
                }
            }

            return Promise.Break;
        }
    }
}