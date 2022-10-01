using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.CommandHandlers
{
    public class UseItemWithItemWalkToSourceHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        public override bool CanHandle(Context context, PlayerUseItemWithItemCommand command)
        {
            if (command.Item.Parent is Tile tile && !command.Player.Tile.Position.IsNextTo(tile.Position) )
            {
                return true;
            }

            return false;
        }

        public override void Handle(Context context, PlayerUseItemWithItemCommand command)
        {
            context.AddCommand(new WalkToUnknownPathCommand(command.Player, (Tile)command.Item.Parent) ).Then(ctx =>
            {
                return ctx.AddCommand(command);

            } ).Then(ctx =>
            {
                OnComplete(ctx);
            } );
        }
    }
}