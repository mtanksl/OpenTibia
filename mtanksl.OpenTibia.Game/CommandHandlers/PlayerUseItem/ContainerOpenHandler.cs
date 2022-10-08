using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class ContainerOpenHandler : CommandHandler<PlayerUseItemCommand>
    {
        public override Promise Handle(Context context, Func<Context, Promise> next, PlayerUseItemCommand command)
        {
            if (command.Item is Container)
            {
                if (command.ContainerId != null)
                {
                    return context.AddCommand(new ContainerReplaceOrCloseCommand(command.Player, (Container)command.Item, command.ContainerId.Value) );
                }
                else
                {
                    return context.AddCommand(new ContainerOpenOrCloseCommand(command.Player, (Container)command.Item) );
                }
            }

            return next(context);
        }
    }
}