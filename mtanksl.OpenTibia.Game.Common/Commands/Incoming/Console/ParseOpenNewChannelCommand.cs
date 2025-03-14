﻿using OpenTibia.Common.Objects;
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

            Guild guild = Context.Server.Guilds.GetGuildThatContainsMember(Player);

            if (guild != null)
            {
                channels.Add(new ChannelDto(0, guild.Name) );
            }

            if (Context.Server.Features.HasFeatureFlag(FeatureFlag.PartyChannel) )
            {
                Party party = Context.Server.Parties.GetPartyThatContainsMember(Player);

                if (party != null)
                {
                    channels.Add(new ChannelDto(1, "Party") );
                }
            }

            if (Player.Rank == Rank.Gamemaster || Player.Rank == Rank.Tutor)
            {
                channels.Add(new ChannelDto(2, "Tutor") );
            }

            
            if (Context.Server.Features.HasFeatureFlag(FeatureFlag.RuleViolationChannel) )
            {
                if (Player.Rank == Rank.Gamemaster)
                {
                    channels.Add(new ChannelDto(3, "Rule Violations") );
                }
            }

            if (Context.Server.Features.HasFeatureFlag(FeatureFlag.GamemasterChannel) )
            {
                if (Player.Rank == Rank.Gamemaster)
                {
                    channels.Add(new ChannelDto(4, "Gamemaster") );
                }
            }
            
            channels.Add(new ChannelDto(5, "Game Chat") );

            if (Player.Rank == Rank.Gamemaster || Player.Vocation != Vocation.None)
            {
                channels.Add(new ChannelDto(6, "Trade") );
            }
            
            if (Player.Rank == Rank.Gamemaster || Player.Vocation == Vocation.None)
            {
                channels.Add(new ChannelDto(7, "Trade-Rookgaard") );
            }

            channels.Add(new ChannelDto(8, "Real Life Chat") );

            channels.Add(new ChannelDto(9, "Help") );

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