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

        public override void Execute(Context context)
        {
            Player reporter = context.Server.GameObjects.GetPlayers()
                .Where(p => p.Name == Name)
                .FirstOrDefault();

            if (reporter != null)
            {
                RuleViolation ruleViolation = context.Server.RuleViolations.GetRuleViolationByReporter(reporter);

                if (ruleViolation != null)
                {
                    if (ruleViolation.Assignee == null)
                    {
                        context.Server.RuleViolations.RemoveRuleViolation(ruleViolation);

                        foreach (var observer in context.Server.Channels.GetChannel(3).GetPlayers() )
                        {
                            context.AddPacket(observer.Client.Connection, new RemoveRuleViolationOutgoingPacket(ruleViolation.Reporter.Name) );
                        }

                        context.AddPacket(ruleViolation.Reporter.Client.Connection, new CloseRuleViolationOutgoingPacket() );

                        base.OnCompleted(context);
                    }
                    else if (ruleViolation.Assignee == Player)
                    {
                        context.Server.RuleViolations.RemoveRuleViolation(ruleViolation);

                        context.AddPacket(ruleViolation.Reporter.Client.Connection, new CloseRuleViolationOutgoingPacket() );

                        base.OnCompleted(context);
                    }
                }
            }
        }
    }
}