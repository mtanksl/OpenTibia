using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ParseOpenedNewChannelCommand : Command
    {
        public ParseOpenedNewChannelCommand(Player player, ushort channelId)
        {
            Player = player;

            ChannelId = channelId;
        }

        public Player Player { get; set; }

        public ushort ChannelId { get; set; }

        public override Promise Execute()
        {
            Channel channel = Context.Server.Channels.GetChannel(ChannelId);
            
            if (channel != null)
            {
                if (channel is PrivateChannel privateChannel)
                {
                    if ( !privateChannel.ContainsPlayer(Player) )
                    {
                        if ( !privateChannel.ContainsInvitation(Player) )
                        {
                            return Promise.Break;
                        }

                        privateChannel.RemoveInvitation(Player);

                        privateChannel.AddPlayer(Player);
                    }

                    Context.AddPacket(Player, new OpenChannelOutgoingPacket(privateChannel.Id, privateChannel.Name) );

                    return Promise.Completed;
                }

                if (channel is GuildChannel guildChannel)
                {
                    if ( !guildChannel.ContainsPlayer(Player) )
                    {
                        guildChannel.AddPlayer(Player);
                    }

                    Context.AddPacket(Player, new OpenChannelOutgoingPacket(guildChannel.Id, guildChannel.Name) );

                    return Promise.Completed;
                }

                if (channel is PartyChannel partyChannel)
                {
                    if ( !partyChannel.ContainsPlayer(Player) )
                    {
                        partyChannel.AddPlayer(Player);
                    }

                    Context.AddPacket(Player, new OpenChannelOutgoingPacket(partyChannel.Id, partyChannel.Name) );

                    return Promise.Completed;
                }

                if (channel.Id == 2)
                {
                    if (Player.Rank != Rank.Tutor && Player.Rank != Rank.Gamemaster)
                    {
                        return Promise.Break;
                    }
                }

                if (channel.Id == 3 || channel.Id == 4)
                {
                    if (Player.Rank != Rank.Gamemaster)
                    {
                        return Promise.Break;
                    }
                }

                if ( !channel.ContainsPlayer(Player) )
                {
                    channel.AddPlayer(Player);
                }

                if (channel.Id == 3)
                {
                    Context.AddPacket(Player, new OpenRuleViolationsChannelOutgoingPacket(channel.Id) );
                    
                    foreach (var ruleViolation in Context.Server.RuleViolations.GetRuleViolations() )
                    {
                        if (ruleViolation.Assignee == null)
                        {
                            Context.AddPacket(Player, new ShowTextOutgoingPacket(0, ruleViolation.Reporter.Name, ruleViolation.Reporter.Level, TalkType.ReportRuleViolationOpen, ruleViolation.Time, ruleViolation.Message) );
                        }
                    }
                }
                else
                {
                    Context.AddPacket(Player, new OpenChannelOutgoingPacket(channel.Id, channel.Name) );
                }

                return Promise.Completed;                
            }

            return Promise.Break;
        }
    }
}