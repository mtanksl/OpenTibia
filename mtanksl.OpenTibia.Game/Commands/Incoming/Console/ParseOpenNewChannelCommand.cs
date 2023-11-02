using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets;
using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;

namespace OpenTibia.Game.Commands
{
    public class ParseOpenNewChannelCommand : Command
    {
        public ParseOpenNewChannelCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        public override Promise Execute()
        {
            List<ChannelDto> channels = new List<ChannelDto>();

            channels.Add(new ChannelDto(0, "Guild") );

            channels.Add(new ChannelDto(1, "Party") );

            if (Player.Rank == Rank.Tutor || Player.Rank == Rank.Gamemaster)
            {
                channels.Add(new ChannelDto(2, "Tutor") );
            }

            if (Player.Rank == Rank.Gamemaster)
            {
                channels.Add(new ChannelDto(3, "Rule Violations") );

                channels.Add(new ChannelDto(4, "Gamemaster") );
            }
            
            channels.Add(new ChannelDto(5, "Game Chat") );

            channels.Add(new ChannelDto(6, "Trade") );

            channels.Add(new ChannelDto(7, "Trade-Rookgaard") );

            channels.Add(new ChannelDto(8, "Real Life Chat") );

            channels.Add(new ChannelDto(9, "Help") );

            channels.Add(new ChannelDto(65535, "Private Chat Channel") );

            foreach (var privateChannel in Context.Server.Channels.GetPrivateChannels() )
            {
                if ( privateChannel.ContainsPlayer(Player) || privateChannel.ContainsInvitation(Player) )
                {
                    channels.Add(new ChannelDto(privateChannel.Id, privateChannel.Name) );
                }
            }

            Context.AddPacket(Player.Client.Connection, new OpenChannelDialogOutgoingPacket(channels) );

            return Promise.Completed;
        }
    }
}