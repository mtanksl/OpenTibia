using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class ParseCloseChannelCommand : Command
    {
        public ParseCloseChannelCommand(Player player, ushort channelId)
        {
            Player = player;

            ChannelId = channelId;
        }

        public Player Player { get; set; }

        public ushort ChannelId { get; set; }

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                Channel channel = context.Server.Channels.GetChannel(ChannelId);

                if (channel != null)
                {
                    if (channel.ContainsPlayer(Player) )
                    {
                        channel.RemovePlayer(Player);
                    }

                    PrivateChannel privateChannel = channel as PrivateChannel;

                    if (privateChannel != null)
                    {
                        if (privateChannel.Owner == Player)
                        {
                            foreach (var observer in privateChannel.GetPlayers().ToList() )
                            {
                                context.AddPacket(observer.Client.Connection, new CloseChannelOutgoingPacket(channel.Id) );

                                privateChannel.RemovePlayer(observer);
                            }

                            foreach (var observer in privateChannel.GetInvitations().ToList() )
                            {
                                privateChannel.RemoveInvitation(observer);
                            }

                            context.Server.Channels.RemoveChannel(privateChannel);
                        }
                    }

                    context.AddPacket(Player.Client.Connection, new CloseChannelOutgoingPacket(channel.Id) );

                    resolve(context);
                }                
            } );
        }
    }
}