using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.CommandHandlers
{
    public class MoveItemWalkToSourceHandler : CommandHandler<PlayerMoveItemCommand>
    {
        public override bool CanHandle(Context context, PlayerMoveItemCommand command)
        {
            if (command.Item.Parent is Tile tile && !command.Player.Tile.Position.IsNextTo(tile.Position) )
            {
                return true;
            }

            return false;
        }

        public override void Handle(Context context, PlayerMoveItemCommand command)
        {
            IContainer beforeContainer = command.Item.Parent;

            byte beforeIndex = beforeContainer.GetIndex(command.Item);

            byte beforeCount = command.Count;

            context.AddCommand(new WalkToUnknownPathCommand(command.Player, (Tile)command.Item.Parent) ).Then(ctx =>
            {
                return Promise.Delay(ctx, Constants.PlayerActionSchedulerEvent(command.Player), Constants.PlayerActionSchedulerEventInterval);

            } ).Then(ctx =>
            {
                IContainer afterContainer = command.Item.Parent;

                byte afterIndex = afterContainer.GetIndex(command.Item);

                byte afterCount = command.Count;

                if (beforeContainer == afterContainer && beforeIndex == afterIndex && beforeCount == afterCount)
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