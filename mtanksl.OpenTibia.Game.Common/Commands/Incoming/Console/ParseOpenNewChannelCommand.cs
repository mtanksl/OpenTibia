using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using OpenTibia.Network.Packets;
using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;

namespace OpenTibia.Game.Commands
{
    public class ParseOpenNewChannelCommand : IncomingCommand
    {
        public ParseOpenNewChannelCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        public override Promise Execute()
        {
            List<ChannelDto> channels = new List<ChannelDto>();

            foreach (var channelConfig in Context.Server.Channels.GetChannelConfigs() )
            {
                if (channelConfig.Flags.Is(ChannelFlags.Guild) )
                {
                    Guild guild = Context.Server.Guilds.GetGuildThatContainsMember(Player);

                    if (guild != null)
                    {
                        channels.Add(new ChannelDto(channelConfig.Id, guild.Name) );
                    }
                }
                else if (channelConfig.Flags.Is(ChannelFlags.Party) )
                {
                    if (Context.Server.Features.HasFeatureFlag(FeatureFlag.PartyChannel) )
                    {
                        Party party = Context.Server.Parties.GetPartyThatContainsMember(Player);

                        if (party != null)
                        {
                            channels.Add(new ChannelDto(channelConfig.Id, channelConfig.Name) );
                        }
                    }
                }
                else if (channelConfig.Flags.Is(ChannelFlags.Tutor) )
                {
                    if (Player.Rank == Rank.Gamemaster || Player.Rank == Rank.Tutor)
                    {
                        channels.Add(new ChannelDto(channelConfig.Id, channelConfig.Name) );
                    }
                }
                else if (channelConfig.Flags.Is(ChannelFlags.RuleViolations) )
                {
                    if (Context.Server.Features.HasFeatureFlag(FeatureFlag.RuleViolationChannel) )
                    {
                        if (Player.Rank == Rank.Gamemaster)
                        {
                            channels.Add(new ChannelDto(channelConfig.Id, channelConfig.Name) );
                        }
                    }
                }
                else if (channelConfig.Flags.Is(ChannelFlags.Gamemaster) )
                {
                    if (Player.Rank == Rank.Gamemaster)
                    {
                        channels.Add(new ChannelDto(channelConfig.Id, channelConfig.Name) );
                    }
                }
                else if (channelConfig.Flags.Is(ChannelFlags.Trade) )
                {
                    if (Player.Rank == Rank.Gamemaster || Player.Vocation != Vocation.None)
                    {
                        channels.Add(new ChannelDto(channelConfig.Id, channelConfig.Name) );
                    }
                }
                else if (channelConfig.Flags.Is(ChannelFlags.TradeRookgaard) )
                {
                    if (Player.Rank == Rank.Gamemaster || Player.Vocation == Vocation.None)
                    {
                        channels.Add(new ChannelDto(channelConfig.Id, channelConfig.Name) );
                    }
                }
                else
                {
                    channels.Add(new ChannelDto(channelConfig.Id, channelConfig.Name) );
                }
            }

            if (Player.Premium)
            {
                channels.Add(new ChannelDto(65535, "Private Chat Channel") );
            }

            foreach (var privateChannel in Context.Server.Channels.GetPrivateChannels() )
            {
                if ( privateChannel.ContainerMember(Player) || privateChannel.ContainsInvitation(Player) )
                {
                    channels.Add(new ChannelDto(privateChannel.Id, privateChannel.Name) );
                }
            }

            Context.AddPacket(Player, new OpenChannelDialogOutgoingPacket(channels) );

            return Promise.Completed;
        }
    }
}