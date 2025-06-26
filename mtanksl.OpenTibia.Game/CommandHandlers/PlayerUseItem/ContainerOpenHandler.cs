using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Linq;

namespace OpenTibia.Game.CommandHandlers
{
    public class ContainerOpenHandler : CommandHandler<PlayerUseItemCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (command.Item is Container container)
            {
                bool open = true;

                foreach (var pair in command.Player.Client.Containers.GetIndexedContainers() )
                {
                    if (pair.Value == container)
                    {
                        command.Player.Client.Containers.CloseContainer(pair.Key);

                        Context.AddPacket(command.Player, new CloseContainerOutgoingPacket(pair.Key) );

                        open = false;
                    }
                }

                if (open)
                {
                    if (command.ContainerId == null)
                    {
                        command.ContainerId = command.Player.Client.Containers.OpenContainer(container);
                    }
                    else
                    {
                        command.Player.Client.Containers.ReplaceContainer(container, command.ContainerId.Value);
                    }

                    //TODO: FeatureFlag.ContainerPagination

                    Context.AddPacket(command.Player, new OpenContainerOutgoingPacket(command.ContainerId.Value, container.Metadata.TibiaId, container.Metadata.Name, container.Metadata.Capacity.Value, container.Parent is Container, true, false, 0, container.GetItems().ToList() ) );
                }               

                return Promise.Completed;
            }

            return next();
        }
    }
}