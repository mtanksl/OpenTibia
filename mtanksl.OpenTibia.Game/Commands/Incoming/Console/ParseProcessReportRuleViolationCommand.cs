using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;
using System;
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

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                Player reporter = context.Server.GameObjects.GetPlayers()
                    .Where(p => p.Name == Name)
                    .FirstOrDefault();
            
                if (reporter != null)
                {
                    RuleViolation ruleViolation = context.Server.RuleViolations.GetRuleViolationByReporter(reporter);

                    if (ruleViolation != null && ruleViolation.Assignee == null)
                    {
                        ruleViolation.Assignee = Player;

                        foreach (var observer in context.Server.Channels.GetChannel(3).GetPlayers() )
                        {
                            context.AddPacket(observer.Client.Connection, new RemoveRuleViolationOutgoingPacket(ruleViolation.Reporter.Name) );
                        }

                        resolve(context);
                    }
                }
            } );
        }
    }
}