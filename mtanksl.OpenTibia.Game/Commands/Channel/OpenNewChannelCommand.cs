using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;
using Channel = OpenTibia.Network.Packets.Channel;

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

            List<Channel> channels = new List<Channel>()
            {
                new Channel(0, "Guild"),

                new Channel(1, "Party"),

                new Channel(2, "Tutor"),

                new Channel(3, "Rule Violations"),

                new Channel(4, "Gamemaster"),

                new Channel(5, "Game Chat"),

                new Channel(6, "Trade"),

                new Channel(7, "Trade-Rookgaard"),

                new Channel(8, "Real Life Chat"),

                new Channel(9, "Help"),

                new Channel(65535, "Private Chat Channel")
            };

            foreach (var privateChannel in server.Channels.GetPrivateChannels() )
            {
                if ( privateChannel.ContainsPlayer(Player) || privateChannel.ContainsInvitation(Player) )
                {
                    channels.Add(new Channel(privateChannel.Id, privateChannel.Name) );
                }
            }

            //Notify

            context.Write(Player.Client.Connection, new OpenChannelDialogOutgoingPacket(channels) );

            base.Execute(server, context);
        }
    }
}