using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;
using Channel = OpenTibia.Network.Packets.Channel;

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
            List<Channel> channels = new List<Channel>();

            channels.Add(new Channel(0, "Guild") );

            channels.Add(new Channel(1, "Party") );

            channels.Add(new Channel(2, "Tutor") );

            if (Player.Vocation == Vocation.Gamemaster)
            {
                channels.Add(new Channel(3, "Rule Violations") );

                channels.Add(new Channel(4, "Gamemaster") );
            }
            
            channels.Add(new Channel(5, "Game Chat") );

            channels.Add(new Channel(6, "Trade") );

            channels.Add(new Channel(7, "Trade-Rookgaard") );

            channels.Add(new Channel(8, "Real Life Chat") );

            channels.Add(new Channel(9, "Help") );

            channels.Add(new Channel(65535, "Private Chat Channel") );

            foreach (var privateChannel in Context.Server.Channels.GetPrivateChannels() )
            {
                if ( privateChannel.ContainsPlayer(Player) || privateChannel.ContainsInvitation(Player) )
                {
                    channels.Add(new Channel(privateChannel.Id, privateChannel.Name) );
                }
            }

            Context.AddPacket(Player.Client.Connection, new OpenChannelDialogOutgoingPacket(channels) );

            return Promise.Completed;
        }
    }
}