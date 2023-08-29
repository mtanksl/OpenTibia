using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class ContainerOpenHandler : CommandHandler<PlayerUseItemCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
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

            return next();
        }
    }
}