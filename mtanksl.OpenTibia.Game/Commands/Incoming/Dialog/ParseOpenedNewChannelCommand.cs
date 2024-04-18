using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ParseOpenedNewChannelCommand : IncomingCommand
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
                if (channel.Id == 0)
                {
                    Guild guild = Context.Server.Guilds.GetGuildThatContainsMember(Player);

                    if (guild == null)
                    {
                        return Promise.Break;
                    }
                }
                else if (channel.Id == 1)
                {
                    Party party = Context.Server.Parties.GetPartyThatContainsMember(Player);

                    if (party == null)
                    {
                        return Promise.Break;
                    }
                }               
                else if (channel.Id == 2)
                {
                    if (Player.Rank != Rank.Gamemaster && Player.Rank != Rank.Tutor)
                    {
                        return Promise.Break;
                    }
                }
                else if (channel.Id == 3 || channel.Id == 4)
                {
                    if (Player.Rank != Rank.Gamemaster)
                    {
                        return Promise.Break;
                    }
                }
                else if (channel.Id == 6)
                {
                    if (Player.Rank != Rank.Gamemaster && Player.Vocation == Vocation.None)
                    {
                        return Promise.Break;
                    }
                }
                else if (channel.Id == 7)
                {
                    if (Player.Rank != Rank.Gamemaster && Player.Vocation != Vocation.None)
                    {
                        return Promise.Break;
                    }
                }
                else if (channel is PrivateChannel privateChannel)
                {
                    if ( !privateChannel.ContainerMember(Player) )
                    {
                        if ( !privateChannel.ContainsInvitation(Player) )
                        {
                            return Promise.Break;
                        }

                        privateChannel.RemoveInvitation(Player);
                    }
                }

                if ( !channel.ContainerMember(Player) )
                {
                    channel.AddMember(Player);
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