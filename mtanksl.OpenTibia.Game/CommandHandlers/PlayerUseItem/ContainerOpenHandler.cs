using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class ContainerOpenHandler : CommandHandler<PlayerUseItemCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (command.Item is Locker locker)
            {
                Container container = Context.Server.Lockers.GetLocker(command.Player.DatabasePlayerId, locker.TownId);

                if (container == null)
                {
                    container = (Container)Context.Server.ItemFactory.Create(2591, 1);

                    Context.Server.Lockers.AddLocker(command.Player.DatabasePlayerId, locker.TownId, container);
                }

                command.Item = container;
            }

            {
                if (command.Item is Container container)
                {
                    if (command.ContainerId != null)
                    {
                        return Context.AddCommand(new ContainerReplaceOrCloseCommand(command.Player, container, command.ContainerId.Value) );
                    }
                    else
                    {
                        return Context.AddCommand(new ContainerOpenOrCloseCommand(command.Player, container) );
                    }
                }
            }

            return next();
        }
    }
}