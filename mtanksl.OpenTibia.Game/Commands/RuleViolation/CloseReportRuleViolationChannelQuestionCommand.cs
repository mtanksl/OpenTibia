using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class CloseReportRuleViolationChannelQuestionCommand : Command
    {
        public CloseReportRuleViolationChannelQuestionCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        public override void Execute(Context context)
        {
            //Arrange
            
            RuleViolation ruleViolation = context.Server.RuleViolations.GetRuleViolationByReporter(Player);

            if (ruleViolation != null)
            {
                if (ruleViolation.Assignee == null)
                {
                    //Act

                    context.Server.RuleViolations.RemoveRuleViolation(ruleViolation);

                    //Notify

                    foreach (var observer in context.Server.Channels.GetChannel(3).GetPlayers() )
                    {
                        context.AddPacket(observer.Client.Connection, new RemoveRuleViolationOutgoingPacket(ruleViolation.Reporter.Name) );
                    }

                    base.Execute(context);
                }
                else
                {
                    //Act

                    context.Server.RuleViolations.RemoveRuleViolation(ruleViolation);

                    //Notify

                    context.AddPacket(ruleViolation.Assignee.Client.Connection, new CancelRuleViolationOutgoingPacket(ruleViolation.Reporter.Name) );

                    base.Execute(context);
                }
            }
        }
    }
}