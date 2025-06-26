using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ParseAnswerReportRuleViolationCommand : IncomingCommand
    {
        public ParseAnswerReportRuleViolationCommand(Player player, string name, string message)
        {
            Player = player;

            Name = name;

            Message = message;
        }

        public Player Player { get; set; }

        public string Name { get; set; }

        public string Message { get; set; }

        public override Promise Execute()
        {
            Player reporter = Context.Server.GameObjects.GetPlayerByName(Name);

            if (reporter != null)
            {
                RuleViolation ruleViolation = Context.Server.RuleViolations.GetRuleViolationByReporter(reporter);

                if (ruleViolation != null && ruleViolation.Assignee == Player)
                {
                    Context.AddPacket(ruleViolation.Reporter, new ShowTextOutgoingPacket(Context.Server.Channels.GenerateStatementId(Player.DatabasePlayerId, Message), ruleViolation.Assignee.Name, ruleViolation.Assignee.Level, MessageMode.RVRAnswer, Message) );

                    return Promise.Completed;
                }
            }

            return Promise.Break;
        }
    }
}