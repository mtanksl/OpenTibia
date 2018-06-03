using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;
using OpenTibia.Web;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class CleanUpCommand : Command
    {
        private Server server;

        public CleanUpCommand(Server server)
        {
            this.server = server;
        }
        
        public Player Player { get; set; }

        public override void Execute(Context context)
        {
            //Arrange

            //Act

            server.CancelQueueForExecution(Constants.PlayerPingSchedulerEvent(Player) );

            server.CancelQueueForExecution(Constants.PlayerWalkSchedulerEvent(Player) );

            foreach (var channel in server.Channels.GetChannels().ToArray() )
            {
                PrivateChannel privateChannel = channel as PrivateChannel;

                if (privateChannel != null)
                {
                    if (privateChannel.ContainsInvitation(Player) )
                    {
                        privateChannel.RemoveInvitation(Player);
                    }
                    else if (privateChannel.ContainsPlayer(Player) )
                    {
                        privateChannel.RemovePlayer(Player);
                    }
                }
                else
                {
                    if (channel.ContainsPlayer(Player) )
                    {
                        channel.RemovePlayer(Player);
                    }
                }
            }

            foreach (var ruleViolation in server.RuleViolations.GetRuleViolations().ToArray() )
            {
                if (ruleViolation.Reporter == Player)
                {
                    if (ruleViolation.Assignee == null)
                    {
                        server.RuleViolations.RemoveRuleViolation(ruleViolation);

                        //Notify

                        foreach (var observer2 in server.Channels.GetChannel(3).GetPlayers() )
                        {
                            context.Response.Write(observer2.Client.Connection, new RemoveRuleViolation(ruleViolation.Reporter.Name) );
                        }
                    }
                    else
                    {
                        server.RuleViolations.RemoveRuleViolation(ruleViolation);

                        //Notify

                        context.Response.Write(ruleViolation.Assignee.Client.Connection, new CancelRuleViolation(ruleViolation.Reporter.Name) );
                    }
                }
                else if (ruleViolation.Assignee == Player)
                {
                    server.RuleViolations.RemoveRuleViolation(ruleViolation);

                    //Notify

                    context.Response.Write(ruleViolation.Reporter.Client.Connection, new CloseRuleViolation() );
                }
            }
        }
    }
}