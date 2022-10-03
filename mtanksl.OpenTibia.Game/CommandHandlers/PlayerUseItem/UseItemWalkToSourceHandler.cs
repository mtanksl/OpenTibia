using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.CommandHandlers
{
    public class UseItemWalkToSourceHandler : CommandHandler<PlayerUseItemCommand>
    {
        public override bool CanHandle(Context context, PlayerUseItemCommand command)
        {
            if (command.Item.Parent is Tile tile && !command.Player.Tile.Position.IsNextTo(tile.Position) )
            {
                return true;
            }

            return false;
        }

        public override void Handle(Context context, PlayerUseItemCommand command)
        {
            IContainer beforeContainer = command.Item.Parent;

            byte beforeIndex = beforeContainer.GetIndex(command.Item);

            context.AddCommand(new WalkToUnknownPathCommand(command.Player, (Tile)command.Item.Parent) ).Then(ctx =>
            {
                return Promise.Delay(ctx, Constants.PlayerActionSchedulerEvent(command.Player), Constants.PlayerActionSchedulerEventInterval);

            } ).Then(ctx =>
            {
                IContainer afterContainer = command.Item.Parent;

                byte afterIndex = afterContainer.GetIndex(command.Item);

                if (beforeContainer == afterContainer && beforeIndex == afterIndex)
                {
                    return ctx.AddCommand(command);
                }

                return null;

            } ).Then(ctx =>
            {
                OnComplete(ctx);
            } );
        }
    }
}