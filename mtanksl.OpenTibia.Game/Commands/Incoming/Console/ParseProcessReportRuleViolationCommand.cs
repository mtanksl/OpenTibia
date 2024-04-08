using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class ParseProcessReportRuleViolationCommand : IncomingCommand
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
            if (Player.Rank == Rank.Gamemaster)
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

                        foreach (var observer in Context.Server.Channels.GetChannel(3).GetMembers() )
                        {
                            Context.AddPacket(observer, new RemoveRuleViolationOutgoingPacket(ruleViolation.Reporter.Name) );
                        }

                        return Promise.Completed;
                    }
                }
            }

            return Promise.Break;
        }
    }
}