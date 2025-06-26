using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using OpenTibia.Network.Packets.Outgoing;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class ParseOpenReportRuleViolationCommand : IncomingCommand
    {
        public ParseOpenReportRuleViolationCommand(Player player, string message)
        {
            Player = player;

            Message = message;
        }

        public Player Player { get; set; }

        public string Message { get; set; }

        public override Promise Execute()
        {
            // ctrl + r

            RuleViolation ruleViolation = Context.Server.RuleViolations.GetRuleViolationByReporter(Player);

            if (ruleViolation == null)
            {
                ruleViolation = new RuleViolation()
                {
                    Reporter = Player,

                    Message = Message
                };

                Context.Server.RuleViolations.AddRuleViolation(ruleViolation);

                var ruleViolationChannel = Context.Server.Channels.GetChannels()
                    .Where(c => c.Flags.Is(ChannelFlags.RuleViolations) )
                    .FirstOrDefault();

                foreach (var observer in ruleViolationChannel.GetMembers() )
                {
                    Context.AddPacket(observer, new ShowTextOutgoingPacket(0, ruleViolation.Reporter.Name, ruleViolation.Reporter.Level, ruleViolation.Time, ruleViolation.Message) );
                }

                return Promise.Completed;
            }

            return Promise.Break;
        }
    }
}