using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ParseOpenedMyPrivateChannelCommand : IncomingCommand
    {
        public ParseOpenedMyPrivateChannelCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        public override Promise Execute()
        {
            if (Player.Premium)
            {
                PrivateChannel privateChannel = Context.Server.Channels.GetPrivateChannel(Player);

                if (privateChannel == null)
                {
                    privateChannel = new PrivateChannel()
                    {
                        Flags = ChannelFlags.PrivateChannel,

                        Owner = Player,

                        Name = Player.Name + "'s Channel"
                    };

                    privateChannel.AddMember(Player);

                    Context.Server.Channels.AddChannel(privateChannel);
                }

                Context.AddPacket(Player, new OpenMyPrivateChannelOutgoingPacket(privateChannel.Id, privateChannel.Name) );
            }

            return Promise.Completed;
        }
    }
}