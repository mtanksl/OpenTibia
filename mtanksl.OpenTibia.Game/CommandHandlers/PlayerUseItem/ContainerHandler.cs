using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.CommandHandlers
{
    public class ContainerHandler : CommandHandler<PlayerUseItemCommand>
    {
        public override bool CanHandle(Context context, PlayerUseItemCommand command)
        {
            if (command.Item is Container)
            {
                return true;
            }

            return false;
        }

        public override void Handle(Context context, PlayerUseItemCommand command)
        {
            if (command.ContainerId != null)
            {
                context.AddCommand(new ContainerReplaceOrCloseCommand(command.Player, (Container)command.Item, command.ContainerId.Value) ).Then(ctx =>
                {
                    OnComplete(ctx);
                } );
            }
            else
            {
                context.AddCommand(new ContainerOpenOrCloseCommand(command.Player, (Container)command.Item) ).Then(ctx =>
                {
                    OnComplete(ctx);
                } );
            }
        }
    }
}