using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class PlayerDestroyChannelCloseHandler : CommandHandler<PlayerDestroyCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerDestroyCommand command)
        {
            return next().Then( () =>
            {
                foreach (var channel in Context.Server.Channels.GetChannels() )
                {
                    if (channel is PrivateChannel privateChannel)
                    {
                        if (privateChannel.Owner == command.Player)
                        {
                            //TODO
                        }

                        if (privateChannel.ContainsInvitation(command.Player) )
                        {
                            //TODO
                        }
                    }

                    if (channel.ContainsPlayer(command.Player))
                    {
                        //TODO
                    }
                }

                return Promise.Completed;
            } );
        }
    }
}