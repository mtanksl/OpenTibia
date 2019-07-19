using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;

namespace OpenTibia.Game.Commands
{
    public class OpenNewChannelCommand : Command
    {
        public OpenNewChannelCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            List<Network.Packets.Channel> channels = new List<Network.Packets.Channel>()
            {
                new Network.Packets.Channel(0, "Guild"),

                new Network.Packets.Channel(1, "Party"),

                new Network.Packets.Channel(2, "Tutor"),

                new Network.Packets.Channel(3, "Rule Violations"),

                new Network.Packets.Channel(4, "Gamemaster"),

                new Network.Packets.Channel(5, "Game chat"),

                new Network.Packets.Channel(6, "Trade"),

                new Network.Packets.Channel(7, "Trade-Rookgaard"),

                new Network.Packets.Channel(8, "Real Life Chat"),

                new Network.Packets.Channel(9, "Help"),

                new Network.Packets.Channel(65535, "Private Chat Channel")
            };

            foreach (var privateChannel in server.Channels.GetPrivateChannels() )
            {
                if (privateChannel.ContainsInvitation(Player) || privateChannel.ContainsPlayer(Player) )
                {
                    channels.Add(new Network.Packets.Channel(privateChannel.Id, privateChannel.Name) );
                }
            }
            
            //Act

            //Notify

            context.Write(Player.Client.Connection, new OpenChannelDialogOutgoingPacket(channels) );
        }
    }
}