using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class OpenedMyPrivateChannelCommand : Command
    {
        public OpenedMyPrivateChannelCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        public override void Execute(Context context)
        {
            //Arrange

            PrivateChannel privateChannel = context.Server.Channels.GetPrivateChannelByOwner(Player);

            if (privateChannel == null)
            {
                //Act

                privateChannel = new PrivateChannel()
                {
                    Owner = Player,

                    Name = Player.Name + "'s channel"
                };

                privateChannel.AddPlayer(Player);

                context.Server.Channels.AddChannel(privateChannel);
            }

            //Notify

            context.AddPacket(Player.Client.Connection, new OpenMyPrivateChannelOutgoingPacket(privateChannel.Id, privateChannel.Name) );

            base.Execute(context);
        }
    }
}