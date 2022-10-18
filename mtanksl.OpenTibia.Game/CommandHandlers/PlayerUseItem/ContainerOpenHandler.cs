using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class ContainerOpenHandler : CommandHandler<PlayerUseItemCommand>
    {
        public override Promise Handle(Context context, Func<Context, Promise> next, PlayerUseItemCommand command)
        {
            if (command.Item is Locker locker)
            {
                Container container = context.Server.Lockers.GetLocker(command.Player.DatabasePlayerId, locker.TownId);

                if (container == null)
                {
                    container = (Container)context.Server.ItemFactory.Create(2591, 1);

                    context.Server.Lockers.AddLocker(command.Player.DatabasePlayerId, locker.TownId, container);
                }

                command.Item = container;
            }

            {
                if (command.Item is Container container)
                {
                    if (command.ContainerId != null)
                    {
                        return context.AddCommand(new ContainerReplaceOrCloseCommand(command.Player, container, command.ContainerId.Value) );
                    }
                    else
                    {
                        return context.AddCommand(new ContainerOpenOrCloseCommand(command.Player, container) );
                    }
                }
            }

            return next(context);
        }
    }
}