using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Linq;

namespace OpenTibia.Game.CommandHandlers
{
    public class PlayerDestroyChannelCloseHandler : CommandHandler<PlayerDestroyCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerDestroyCommand command)
        {
            return next().Then( () =>
            {
                foreach (var channel in Context.Server.Channels.GetChannels().ToList() )
                {
                    if (channel.ContainsPlayer(command.Player) )
                    {
                        channel.RemovePlayer(command.Player);

                        Context.AddPacket(command.Player.Client.Connection, new CloseChannelOutgoingPacket(channel.Id) );
                    }

                    if (channel is PrivateChannel privateChannel)
                    {
                        if (privateChannel.ContainsInvitation(command.Player) )
                        {
                            privateChannel.RemovePlayer(command.Player);
                        }

                        if (privateChannel.Owner == command.Player)
                        {
                            foreach (var observer in privateChannel.GetPlayers().ToList() )
                            {
                                Context.AddPacket(observer.Client.Connection, new CloseChannelOutgoingPacket(channel.Id) );

                                privateChannel.RemovePlayer(observer);
                            }

                            foreach (var observer in privateChannel.GetInvitations().ToList() )
                            {
                                privateChannel.RemoveInvitation(observer);
                            }

                            Context.Server.Channels.RemoveChannel(privateChannel);
                        }
                    }
                }

                return Promise.Completed;
            } );
        }
    }
}