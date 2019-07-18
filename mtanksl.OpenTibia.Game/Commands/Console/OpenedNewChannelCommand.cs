using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class OpenedNewChannelCommand : Command
    {
        public OpenedNewChannelCommand(Player player, ushort channelId)
        {
            Player = player;

            ChannelId = channelId;
        }

        public Player Player { get; set; }

        public ushort ChannelId { get; set; }

        public override void Execute(Server server, CommandContext context)
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

                    context.Write(Player.Client.Connection, new OpenRuleViolationsChannel(channel.Id) );
                    
                    foreach (var ruleViolation in server.RuleViolations.GetRuleViolations() )
                    {
                        if (ruleViolation.Assignee == null)
                        {
                            context.Write(Player.Client.Connection, new ShowText(0, ruleViolation.Reporter.Name, ruleViolation.Reporter.Level, TalkType.ReportRuleViolationOpen, ruleViolation.Time, ruleViolation.Message) );
                        }
                    }
                }
                else
                {
                    //Notify

                    context.Write(Player.Client.Connection, new OpenChannel(channel.Id, channel.Name) );
                }
            }
        }
    }
}