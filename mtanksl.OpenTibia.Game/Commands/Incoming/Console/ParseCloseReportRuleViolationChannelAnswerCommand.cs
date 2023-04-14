using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class ParseCloseReportRuleViolationChannelAnswerCommand : Command
    {
        public ParseCloseReportRuleViolationChannelAnswerCommand(Player player, string name)
        {
            Player = player;

            Name = name;
        }

        public Player Player { get; set; }

        public string Name { get; set; }

        public override Promise Execute()
        {
            Player reporter = Context.Server.GameObjects.GetPlayers()
                .Where(p => p.Name == Name)
                .FirstOrDefault();

            if (reporter != null)
            {
                RuleViolation ruleViolation = Context.Server.RuleViolations.GetRuleViolationByReporter(reporter);

                if (ruleViolation != null)
                {
                    if (ruleViolation.Assignee == null)
                    {
                        Context.Server.RuleViolations.RemoveRuleViolation(ruleViolation);

                        foreach (var observer in Context.Server.Channels.GetChannel(3).GetPlayers() )
                        {
                            Context.AddPacket(observer.Client.Connection, new RemoveRuleViolationOutgoingPacket(ruleViolation.Reporter.Name) );
                        }

                        Context.AddPacket(ruleViolation.Reporter.Client.Connection, new CloseRuleViolationOutgoingPacket() );

                        return Promise.Completed;
                    }
                    else if (ruleViolation.Assignee == Player)
                    {
                        Context.Server.RuleViolations.RemoveRuleViolation(ruleViolation);

                        Context.AddPacket(ruleViolation.Reporter.Client.Connection, new CloseRuleViolationOutgoingPacket() );

                        return Promise.Completed;
                    }
                }
            }

            return Promise.Break;
        }
    }
}