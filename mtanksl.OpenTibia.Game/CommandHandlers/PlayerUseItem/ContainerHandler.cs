using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.CommandHandlers
{
    public class ContainerHandler : CommandHandler<PlayerUseItemCommand>
    {
        public override bool CanHandle(PlayerUseItemCommand command, Server server)
        {
            if (command.Item is Container)
            {
                return true;
            }

            return false;
        }

        public override Command Handle(PlayerUseItemCommand command, Server server)
        {
            if (command.ContainerId != null)
            {
                return new ContainerReplaceOrCloseCommand(command.Player, (Container)command.Item, command.ContainerId.Value);
            }

            return new ContainerOpenOrCloseCommand(command.Player, (Container)command.Item);
        }
    }
}