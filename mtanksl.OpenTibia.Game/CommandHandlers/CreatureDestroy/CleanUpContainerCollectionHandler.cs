using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class CleanUpContainerCollectionHandler : CommandHandler<CreatureDestroyCommand>
    {
        public override Promise Handle(Func<Promise> next, CreatureDestroyCommand command)
        {
            if (command.Creature is Player player)
            {
                return next().Then( () =>
                {
                    foreach (var pair in player.Client.Containers.GetIndexedContainers() )
                    {
                        player.Client.Containers.CloseContainer(pair.Key);
                         
                        Context.AddPacket(player, new CloseContainerOutgoingPacket(pair.Key) );
                    }

                    return Promise.Completed;
                } );
            }

            return next();
        }
    }
}