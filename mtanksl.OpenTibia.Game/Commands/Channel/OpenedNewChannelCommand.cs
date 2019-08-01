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

        public override void Execute(Server server, Context context)
        {
            //Arrange

            Channel channel = server.Channels.GetChannel(ChannelId);
            
            if (channel != null)
            {
                PrivateChannel privateChannel = channel as PrivateChannel;

                if (privateChannel != null)
                {
                    if ( !privateChannel.ContainsPlayer(Player) )
                    {
                        if ( !privateChannel.ContainsInvitation(Player) )
                        {
                            return;
                        }

                        //Act

                        privateChannel.RemoveInvitation(Player);

                        privateChannel.AddPlayer(Player);
                    }

                    //Notify

                    context.Write(Player.Client.Connection, new OpenChannelOutgoingPacket(privateChannel.Id, privateChannel.Name) );

                    base.Execute(server, context);
                }
                else
                {
                    GuildChannel guildChannel = channel as GuildChannel;

                    if (guildChannel != null)
                    {
                        if ( !guildChannel.ContainsPlayer(Player) )
                        {
                            //Act

                            guildChannel.AddPlayer(Player);
                        }

                        //Notify

                        context.Write(Player.Client.Connection, new OpenChannelOutgoingPacket(guildChannel.Id, guildChannel.Name) );

                        base.Execute(server, context);
                    }
                    else
                    {
                        PartyChannel partyChannel = channel as PartyChannel;

                        if (partyChannel != null)
                        {
                            if ( !partyChannel.ContainsPlayer(Player) )
                            {
                                //Act

                                partyChannel.AddPlayer(Player);
                            }

                            //Notify

                            context.Write(Player.Client.Connection, new OpenChannelOutgoingPacket(partyChannel.Id, partyChannel.Name) );

                            base.Execute(server, context);
                        }
                        else
                        {
                            if ( !channel.ContainsPlayer(Player) )
                            {
                                //Act

                                channel.AddPlayer(Player);
                            }

                            //Notify

                            if (channel.Id == 3)
                            {
                                context.Write(Player.Client.Connection, new OpenRuleViolationsChannelOutgoingPacket(channel.Id) );
                    
                                foreach (var ruleViolation in server.RuleViolations.GetRuleViolations() )
                                {
                                    if (ruleViolation.Assignee == null)
                                    {
                                        context.Write(Player.Client.Connection, new ShowTextOutgoingPacket(0, ruleViolation.Reporter.Name, ruleViolation.Reporter.Level, TalkType.ReportRuleViolationOpen, ruleViolation.Time, ruleViolation.Message) );
                                    }
                                }
                            }
                            else
                            {
                                context.Write(Player.Client.Connection, new OpenChannelOutgoingPacket(channel.Id, channel.Name) );
                            }

                            base.Execute(server, context);
                        }
                    }                    
                }
            }
        }
    }
}