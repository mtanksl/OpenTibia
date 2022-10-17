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
                command.Item = context.Server.Lockers.GetLocker(context, command.Player.DatabasePlayerId, locker.TownId);
            }

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

            return next(context);
        }
    }
}