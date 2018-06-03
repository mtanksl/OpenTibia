using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using OpenTibia.Web;

namespace OpenTibia.Game.Commands
{
    public class OpenedNewChannelCommand : Command
    {
        private Server server;

        public OpenedNewChannelCommand(Server server)
        {
            this.server = server;
        }

        public Player Player { get; set; }

        public ushort ChannelId { get; set; }

        public override void Execute(Context context)
        {
            //Arrange

            Channel channel = server.Channels.GetChannel(ChannelId);
            
            //Act
            
            if (channel != null)
            {
                PrivateChannel privateChannel = channel as PrivateChannel;

                if (privateChannel != null)
                {
                    if (privateChannel.ContainsInvitation(Player) )
                    {
                        privateChannel.RemoveInvitation(Player);

                        privateChannel.AddPlayer(Player);
                    }
                    else if (privateChannel.ContainsPlayer(Player) )
                    {
                        //
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    if (channel.ContainsPlayer(Player) )
                    {
                        //
                    }
                    else
                    { 
                        channel.AddPlayer(Player);
                    }
                }

                if (channel.Id == 3)
                {
                    //Notify

                    context.Response.Write(Player.Client.Connection, new OpenRuleViolationsChannel(channel.Id) );
                    
                    foreach (var ruleViolation in server.RuleViolations.GetRuleViolations() )
                    {
                        if (ruleViolation.Assignee == null)
                        {
                            context.Response.Write(Player.Client.Connection, new ShowText(0, ruleViolation.Reporter.Name, ruleViolation.Reporter.Level, TalkType.ReportRuleViolationOpen, ruleViolation.Time, ruleViolation.Message) );
                        }
                    }
                }
                else
                {
                    //Notify

                    context.Response.Write(Player.Client.Connection, new OpenChannel(channel.Id, channel.Name) );
                }
            }
        }
    }
}