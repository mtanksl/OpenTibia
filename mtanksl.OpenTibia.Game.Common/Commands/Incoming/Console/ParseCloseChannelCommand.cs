﻿using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using OpenTibia.Network.Packets.Outgoing;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class ParseCloseChannelCommand : IncomingCommand
    {
        public ParseCloseChannelCommand(Player player, ushort channelId)
        {
            Player = player;

            ChannelId = channelId;
        }

        public Player Player { get; set; }

        public ushort ChannelId { get; set; }

        public override Promise Execute()
        {
            Channel channel = Context.Server.Channels.GetChannel(ChannelId);

            if (channel != null)
            {
                if (channel.ContainerMember(Player) )
                {
                    channel.RemoveMember(Player);
                                  
                    Context.AddPacket(Player, new CloseChannelOutgoingPacket(channel.Id) );

                    if (channel is PrivateChannel privateChannel)
                    {
                        if (privateChannel.Owner == Player)
                        {
                            foreach (var observer in privateChannel.GetMembers().ToArray() )
                            {
                                privateChannel.RemoveMember(observer);

                                Context.AddPacket(observer, new CloseChannelOutgoingPacket(channel.Id) );
                            }

                            foreach (var observer in privateChannel.GetInvitations().ToArray() )
                            {
                                privateChannel.RemoveInvitation(observer);
                            }

                            Context.Server.Channels.RemoveChannel(privateChannel);
                        }
                    }
                }

                return Promise.Completed;
            }

            return Promise.Break;
        }
    }
}