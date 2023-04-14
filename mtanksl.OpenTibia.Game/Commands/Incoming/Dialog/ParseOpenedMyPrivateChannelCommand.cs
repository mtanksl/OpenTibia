using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ParseOpenedMyPrivateChannelCommand : Command
    {
        public ParseOpenedMyPrivateChannelCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        public override Promise Execute()
    {
            PrivateChannel privateChannel = Context.Server.Channels.GetPrivateChannelByOwner(Player);

            if (privateChannel == null)
            {
                privateChannel = new PrivateChannel()
                {
                    Owner = Player,

                    Name = Player.Name + "'s channel"
                };

                privateChannel.AddPlayer(Player);

                Context.Server.Channels.AddChannel(privateChannel);
            }

            Context.AddPacket(Player.Client.Connection, new OpenMyPrivateChannelOutgoingPacket(privateChannel.Id, privateChannel.Name) );

            return Promise.Completed;
        }
    }
}